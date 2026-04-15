using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaoCattles.Modules.Products.Application.Services;
using RaoCattles.Modules.Products.Presentation.Requests;

namespace RaoCattles.Modules.Products.Presentation.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Admin")]
public class AdminProductsController(IProductService productService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        var id = await productService.CreateAsync(
            request.Name, request.Breed, request.Description,
            request.Age, request.Weight, request.Color, request.Teeth,
            request.Price,
            request.Image1, request.Image2, request.Image3, ct);

        return CreatedAtAction("GetById", "PublicProducts", new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductRequest request, CancellationToken ct)
    {
        await productService.UpdateAsync(
            id,
            request.Name, request.Breed, request.Description,
            request.Age, request.Weight, request.Color, request.Teeth,
            request.Price, request.Sold,
            request.Image1, request.Image2, request.Image3, ct);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await productService.DeleteAsync(id, ct);
        return NoContent();
    }
}
