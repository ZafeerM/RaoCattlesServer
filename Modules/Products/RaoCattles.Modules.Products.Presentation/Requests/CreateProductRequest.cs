using Microsoft.AspNetCore.Http;

namespace RaoCattles.Modules.Products.Presentation.Requests;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Age { get; set; }
    public double Weight { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Teeth { get; set; }
    public decimal Price { get; set; }
    public IFormFile Image1 { get; set; } = null!;
    public IFormFile Image2 { get; set; } = null!;
    public IFormFile Image3 { get; set; } = null!;
}
