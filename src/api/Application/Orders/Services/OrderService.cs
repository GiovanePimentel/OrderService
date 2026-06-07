
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<ResponseOrder> CreateOrder(CreateOrderRequest TryOrder)
    {
        ResponseOrder resposta = new ResponseOrder();
        List<CreateOrderItemRequest> ListaItens = TryOrder.Items;

        resposta.Approved = true;

        ResponseItems respostaItems = await ValidateItems(ListaItens);

        if (respostaItems == null)
        {
            resposta.Approved = false;
            resposta.Message = "Error";
            return resposta;
        }
        if (respostaItems.Approved == false)
        {
            resposta.Approved = false;
            resposta.Message = respostaItems.Message;
            return resposta;
        }

        //itens nao será nulo porque se foi aprovado, contem itens;
        Order ordem = new(TryOrder.CustomerId, OrderStatus.Placed, TryOrder.Currency, respostaItems.Items);

        try
        {
            _db.Orders.Add(ordem);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            resposta.Approved = false;
            resposta.Message = ex.Message;
            return resposta;
        }

        respostaItems.Items.ForEach(x => x.OrderID = ordem.Id);
        ordem.Itens = respostaItems.Items;
        resposta.order = ordem;

        return resposta;
    }
    public async Task<ResponseItems> ValidateItems(List<CreateOrderItemRequest> lista)
    {
        ResponseItems retornaProduto = new ResponseItems();
        retornaProduto.Approved = true;
        retornaProduto.Items = new List<OrderItem>();

        var ids = lista.Select(x => x.ProductId).ToList();

        try
        {
            var produtos = await _db.Products.Where(x => ids.Contains(x.Id)).ToListAsync();

            var encontrados = produtos.Select(x => x.Id);
            var faltantes = ids.Except(encontrados).ToList();

            if (faltantes.Any())
            {
                retornaProduto.Approved = false;

                retornaProduto.Message = $"Product not found: {string.Join(",", faltantes)}";

                return retornaProduto;
            }

            foreach (var produto in produtos)
            {
                if (produto.AvailableQuantity == 0)
                {
                    retornaProduto.Message = "Product empty Stock. Product " + produto.Id.ToString();
                    retornaProduto.Approved = false;
                    break;
                }
                if (produto.AvailableQuantity < lista.Where(x => x.ProductId == produto.Id).Sum(x => x.Quantity))
                {
                    retornaProduto.Message = "Product no Stock. Product " + produto.Id.ToString() + ", AvailableQuantity " + produto.AvailableQuantity.ToString();
                    retornaProduto.Approved = false;
                    return retornaProduto;
                }
                retornaProduto.Items.Add(new OrderItem(produto.Id, produto.UnitPrice, lista.Where(x => x.ProductId == produto.Id).Sum(x => x.Quantity)));
            }
        }
        catch (System.Exception ex)
        {
            retornaProduto.Message = ex.Message;
            retornaProduto.Approved = false;
            return retornaProduto;
        }
        if (retornaProduto.Items.Count == 0)
        {
            retornaProduto.Approved = false;
            retornaProduto.Message = "No valid products";
        }

        return retornaProduto;
    }
    public async Task<ConfirmOrdem> CofirmOrder(int OrderId)
    {
        ConfirmOrdem confirm = new ConfirmOrdem();
        confirm.Approved = true;
        try
        {

            Order? OrdemSelecionada = await _db.Orders.Where(x => x.Id == OrderId).FirstOrDefaultAsync();
            if (OrdemSelecionada == null)
            {
                confirm.Approved = false;
                return confirm;
            }

            OrdemSelecionada.Itens = await _db.Items.Where(x => x.OrderID == OrderId).ToListAsync();

            if (OrdemSelecionada.Status == OrderStatus.Canceled)
            {
                confirm.Approved = false;
                confirm.Message = "Canceled order cannot be confirmed";
                return confirm;
            }
            if (OrdemSelecionada.Status == OrderStatus.Confirmed)
            {
                confirm.Approved = true;
                confirm.Message = "Order confirmed !";
                return confirm;

            }
            List<CreateOrderItemRequest> it = new List<CreateOrderItemRequest>();
            foreach (var ite in OrdemSelecionada.Itens)
            {
                it.Add(new CreateOrderItemRequest(ite.ProductId, ite.Quantity));
            }
            ResponseItems respostaItems = await ValidateItems(it);
            if (respostaItems.Approved == false)
            {
                confirm.Approved = false;
                confirm.Message = respostaItems.Message;
                return confirm;
            }

            var ids = OrdemSelecionada.Itens.Select(x => x.ProductId).ToList();

            var produtos = await _db.Products.Where(x => ids.Contains(x.Id)).ToListAsync();

            foreach (var produto in produtos)
            {
                var item = OrdemSelecionada.Itens.First(x => x.ProductId == produto.Id);

                var quantidade = OrdemSelecionada.Itens.Where(x => x.ProductId == produto.Id).Sum(x => x.Quantity);

                produto.AvailableQuantity -= quantidade;

            }
            OrdemSelecionada.Status = OrderStatus.Confirmed;


            await _db.SaveChangesAsync();

            confirm.Message = "Order Confirmed !";
        }
        catch (System.Exception ex)
        {
            confirm.Approved = false;
            confirm.Message = ex.Message;
            return confirm;
        }


        return confirm;
    }
    public async Task<ConfirmOrdem> CancelOrder(int OrderId)
    {
        ConfirmOrdem confirm = new ConfirmOrdem();
        confirm.Approved = true;
        try
        {
            Order? OrdemSelecionada = await _db.Orders.Where(x => x.Id == OrderId).FirstOrDefaultAsync();
            if (OrdemSelecionada == null)
            {
                confirm.Approved = false;
                confirm.Message = "non-existent order.";
                return confirm;
            }

            if (OrdemSelecionada.Status == OrderStatus.Canceled)
            {
                confirm.Approved = true;
                confirm.Message = "Order Canceled.";
                return confirm;
            }
            if (OrdemSelecionada.Status == OrderStatus.Placed)
            {
                confirm.Approved = true;
                confirm.Message = "Order Canceled.";
                return confirm;
            }

            OrdemSelecionada.Itens = await _db.Items.Where(x => x.OrderID == OrderId).ToListAsync();

            if (OrdemSelecionada.Status == OrderStatus.Canceled)
            {
                confirm.Approved = false;
            }
            if (OrdemSelecionada.Status == OrderStatus.Confirmed)
            {
                confirm.Approved = true;
                confirm.Message = "Order confirmed !";
                return confirm;

            }
            List<CreateOrderItemRequest> it = new List<CreateOrderItemRequest>();
            foreach (var ite in OrdemSelecionada.Itens)
            {
                it.Add(new CreateOrderItemRequest(ite.ProductId, ite.Quantity));
            }
            ResponseItems respostaItems = await ValidateItems(it);
            if (respostaItems.Approved == false)
            {
                confirm.Approved = false;
                confirm.Message = respostaItems.Message;
                return confirm;
            }

            var ids = OrdemSelecionada.Itens.Select(x => x.Id).ToList();

            var produtos = await _db.Products.Where(x => ids.Contains(x.Id)).ToListAsync();

            foreach (var produto in produtos)
            {
                produto.AvailableQuantity = produto.AvailableQuantity - OrdemSelecionada.Itens.Where(x => x.ProductId == produto.Id).Select(x => x.Quantity).FirstOrDefault();
            }
            OrdemSelecionada.Status = OrderStatus.Confirmed;

            foreach (var prod in produtos)
            {
                _db.Products.Update(prod);
            }
            _db.Orders.Update(OrdemSelecionada);

            await _db.SaveChangesAsync();

            confirm.Message = "Order Confirmed !";
        }
        catch (System.Exception ex)
        {
            confirm.Approved = false;
            confirm.Message = ex.Message;
            return confirm;
        }


        return confirm;
    }

}