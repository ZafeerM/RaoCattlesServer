using Microsoft.Extensions.DependencyInjection;
using RaoCattles.BuildingBlocks.Authentication;
using RaoCattles.Modules.Users.Application.Contracts;
using RaoCattles.Modules.Users.Application.Services;
using RaoCattles.Modules.Users.Infrastructure.Repositories;
using RaoCattles.Modules.Users.Infrastructure.Services;

namespace RaoCattles.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, JwtSettings jwtSettings)
    {
        services.AddSingleton(jwtSettings);
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
