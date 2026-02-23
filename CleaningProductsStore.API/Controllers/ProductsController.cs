using CleaningProductsStore.Application.DataTransferObjects;
using CleaningProductsStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleaningProductsStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequestDTO request)
    {
        try
        {
            var id = await _productService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok();
    }

    [HttpGet("by-status")]
    public async Task<IActionResult> GetByStatus([FromQuery] bool isDeleted)
    {
        var result = await _productService.GetByStatusAsync(isDeleted);
        return Ok(result);
    }
}
