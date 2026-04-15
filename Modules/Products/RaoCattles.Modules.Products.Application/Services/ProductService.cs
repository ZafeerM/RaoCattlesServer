using RaoCattles.BuildingBlocks.Exceptions;
using RaoCattles.Modules.Products.Application.Contracts;
using RaoCattles.Modules.Products.Application.Dtos;
using RaoCattles.Modules.Products.Domain.Entities;

namespace RaoCattles.Modules.Products.Application.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<List<ProductResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var products = await repository.GetAllAsync(ct);

        return products.Select(p => new ProductResponse(
            p.Id, p.Name, p.Breed, p.Description,
            p.Age, p.Weight, p.Color, p.Teeth,
            p.Price, p.Sold,
            p.Image1, p.Image2, p.Image3,
            p.CreatedAt, p.UpdatedAt)).ToList();
    }

    public async Task<ProductResponse> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var p = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product with id '{id}' was not found.");

        return new ProductResponse(
            p.Id, p.Name, p.Breed, p.Description,
            p.Age, p.Weight, p.Color, p.Teeth,
            p.Price, p.Sold,
            p.Image1, p.Image2, p.Image3,
            p.CreatedAt, p.UpdatedAt);
    }

    public async Task<string> CreateAsync(string name, string breed, string description, int age, double weight, string color, int teeth, decimal price, string image1, string image2, string image3, CancellationToken ct = default)
    {
        var product = new Product
        {
            Name = name,
            Breed = breed,
            Description = description,
            Age = age,
            Weight = weight,
            Color = color,
            Teeth = teeth,
            Price = price,
            Sold = false,
            Image1 = image1,
            Image2 = image2,
            Image3 = image3,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(product, ct);
        return product.Id;
    }

    public async Task UpdateAsync(string id, string name, string breed, string description, int age, double weight, string color, int teeth, decimal price, bool sold, string image1, string image2, string image3, CancellationToken ct = default)
    {
        var product = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product with id '{id}' was not found.");

        product.Name = name;
        product.Breed = breed;
        product.Description = description;
        product.Age = age;
        product.Weight = weight;
        product.Color = color;
        product.Teeth = teeth;
        product.Price = price;
        product.Sold = sold;
        product.Image1 = image1;
        product.Image2 = image2;
        product.Image3 = image3;
        product.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(product, ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        _ = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product with id '{id}' was not found.");

        await repository.DeleteAsync(id, ct);
    }
}
