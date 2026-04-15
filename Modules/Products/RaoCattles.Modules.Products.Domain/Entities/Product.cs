using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RaoCattles.Modules.Products.Domain.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Breed { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Age { get; set; }

    public double Weight { get; set; }

    public string Color { get; set; } = string.Empty;

    public int Teeth { get; set; }

    public decimal Price { get; set; }

    public bool Sold { get; set; }

    public string Image1 { get; set; } = string.Empty;

    public string Image2 { get; set; } = string.Empty;

    public string Image3 { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
