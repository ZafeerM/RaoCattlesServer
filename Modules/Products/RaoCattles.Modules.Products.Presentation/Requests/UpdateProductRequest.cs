namespace RaoCattles.Modules.Products.Presentation.Requests;

public record UpdateProductRequest(
    string Name,
    string Breed,
    string Description,
    int Age,
    double Weight,
    string Color,
    int Teeth,
    decimal Price,
    bool Sold);
