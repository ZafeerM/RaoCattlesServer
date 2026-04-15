namespace RaoCattles.Modules.Products.Application.Dtos;

public record ProductResponse(
    string Id,
    string Name,
    string Breed,
    string Description,
    int Age,
    double Weight,
    string Color,
    int Teeth,
    decimal Price,
    bool Sold,
    string Image1,
    string Image2,
    string Image3,
    DateTime CreatedAt,
    DateTime UpdatedAt);
