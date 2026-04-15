using Microsoft.AspNetCore.Mvc;
using RaoCattles.Modules.Products.Application.Services;

namespace RaoCattles.Modules.Products.Presentation.Controllers;

[ApiController]
[Route("api/products")]
public class PublicProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var products = await productService.GetAllAsync(ct);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var product = await productService.GetByIdAsync(id, ct);
        return Ok(product);
    }
}
