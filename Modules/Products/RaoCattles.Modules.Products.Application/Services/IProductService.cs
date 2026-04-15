using RaoCattles.Modules.Products.Application.Dtos;

namespace RaoCattles.Modules.Products.Application.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetAllAsync(CancellationToken ct = default);
    Task<ProductResponse> GetByIdAsync(string id, CancellationToken ct = default);
    Task<string> CreateAsync(string name, string breed, string description, int age, double weight, string color, int teeth, decimal price, string image1, string image2, string image3, CancellationToken ct = default);
    Task UpdateAsync(string id, string name, string breed, string description, int age, double weight, string color, int teeth, decimal price, bool sold, string image1, string image2, string image3, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
}
