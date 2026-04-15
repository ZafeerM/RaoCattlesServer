namespace RaoCattles.Modules.Products.Presentation.Requests;

public record CreateProductRequest(
    string Name,
    string Breed,
    string Description,
    int Age,
    double Weight,
    string Color,
    int Teeth,
    decimal Price,
    string Image1,
    string Image2,
    string Image3);
