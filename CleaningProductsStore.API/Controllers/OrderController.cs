using CleaningProductsStore.Application.DataTransferObjects;
using CleaningProductsStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleaningProductsStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequestDto request)
    {
        try
        {
            var id = await _orderService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok();
    }
}
