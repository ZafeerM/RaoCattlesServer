using MongoDB.Driver;
using RaoCattles.Modules.Products.Application.Contracts;
using RaoCattles.Modules.Products.Domain.Entities;

namespace RaoCattles.Modules.Products.Infrastructure.Repositories;

public class ProductRepository(IMongoDatabase database) : IProductRepository
{
    private readonly IMongoCollection<Product> _collection =
        database.GetCollection<Product>("products");

    public async Task<List<Product>> GetAllAsync(CancellationToken ct = default)
        => await _collection.Find(_ => true).ToListAsync(ct);

    public async Task<Product?> GetByIdAsync(string id, CancellationToken ct = default)
        => await _collection.Find(p => p.Id == id).FirstOrDefaultAsync(ct);

    public async Task CreateAsync(Product product, CancellationToken ct = default)
        => await _collection.InsertOneAsync(product, cancellationToken: ct);

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        product.UpdatedAt = DateTime.UtcNow;
        await _collection.ReplaceOneAsync(p => p.Id == product.Id, product, cancellationToken: ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
        => await _collection.DeleteOneAsync(p => p.Id == id, ct);
}
