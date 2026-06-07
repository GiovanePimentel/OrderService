using FluentValidation;

public class OrderItemValidator : AbstractValidator<CreateOrderItemRequest>
{
    public OrderItemValidator()
    {
        RuleFor(CreateOrderItemRequest => CreateOrderItemRequest.ProductId).GreaterThan(0).WithMessage("O ID do produto deve ser maior que zero.");
        RuleFor(CreateOrderItemRequest => CreateOrderItemRequest.ProductId).NotEmpty().WithMessage("O id do produto não pode ser vazio.")
                                         .GreaterThan(0).WithMessage("O id do produto deve ser um inteiro maior que 0.");
        RuleFor(CreateOrderItemRequest => CreateOrderItemRequest.Quantity).GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
    }
}