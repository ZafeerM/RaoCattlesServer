using RaoCattles.Modules.Products.Domain.Entities;

namespace RaoCattles.Modules.Products.Application.Contracts;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(CancellationToken ct = default);
    Task<Product?> GetByIdAsync(string id, CancellationToken ct = default);
    Task CreateAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
}
