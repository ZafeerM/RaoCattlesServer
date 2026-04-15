using RaoCattles.BuildingBlocks.Exceptions;
using RaoCattles.Modules.Products.Application.Contracts;
using RaoCattles.Modules.Products.Application.Dtos;
using RaoCattles.Modules.Products.Domain.Entities;

namespace RaoCattles.Modules.Products.Application.Services;

public class ProductService(IProductRepository repository, IImageService imageService) : IProductService
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

    public async Task<string> CreateAsync(string name, string breed, string description, int age, double weight, string color, int teeth, decimal price, ImageInput image1, ImageInput image2, ImageInput image3, CancellationToken ct = default)
    {
        var publicId1 = await ProcessAndUploadAsync(image1, ct);
        var publicId2 = await ProcessAndUploadAsync(image2, ct);
        var publicId3 = await ProcessAndUploadAsync(image3, ct);

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
            Image1 = publicId1,
            Image2 = publicId2,
            Image3 = publicId3,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(product, ct);
        return product.Id;
    }

    public async Task UpdateAsync(string id, string name, string breed, string description, int age, double weight, string color, int teeth, decimal price, bool sold, CancellationToken ct = default)
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
        product.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(product, ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var product = await repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product with id '{id}' was not found.");

        await imageService.DeleteAsync(product.Image1, ct);
        await imageService.DeleteAsync(product.Image2, ct);
        await imageService.DeleteAsync(product.Image3, ct);

        await repository.DeleteAsync(id, ct);
    }

    private async Task<string> ProcessAndUploadAsync(ImageInput image, CancellationToken ct)
    {
        using var compressed = await imageService.CompressAsync(image.Stream, image.ContentType, image.Size, ct);
        return await imageService.UploadAsync(compressed, image.FileName, ct);
    }
}
