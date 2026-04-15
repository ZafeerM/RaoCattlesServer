using RaoCattles.Modules.Users.Application.Dtos;

namespace RaoCattles.Modules.Users.Application.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default);
}
