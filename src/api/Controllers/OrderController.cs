using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;


    public OrdersController(
        OrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request)
    {
        var result =
            await _orderService.CreateOrder(request);

        if (!result.Approved)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.order.Id },
            result.order);
    }

    [Authorize]
    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> Confirm(
        int id)
    {
        var result =
            await _orderService.CofirmOrder(id);

        if (!result.Approved)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(
            int id)
    {
        var result =
            await _orderService.CancelOrder(id);

        if (!result.Approved)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
            int id)
    {
        var result =
            await _orderService.GetOrder(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
    {
        var result = await _orderService.GetOrders(page, pageSize);

        return Ok(result);
    }

}
