using Microsoft.Extensions.DependencyInjection;
using RaoCattles.Modules.Products.Application.Contracts;
using RaoCattles.Modules.Products.Application.Services;
using RaoCattles.Modules.Products.Infrastructure.Repositories;
using RaoCattles.Modules.Products.Infrastructure.Services;

namespace RaoCattles.Modules.Products.Infrastructure;

public static class ProductsModule
{
    public static IServiceCollection AddProductsModule(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
