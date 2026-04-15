using RaoCattles.Modules.Users.Application.Contracts;
using RaoCattles.Modules.Users.Application.Dtos;

namespace RaoCattles.Modules.Users.Application.Services;

public class AuthService(IAdminUserRepository userRepository, IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        var user = await userRepository.GetByUsernameAsync(username, ct)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = jwtTokenService.GenerateToken(user);
        return new LoginResponse(token, DateTime.UtcNow.AddHours(1));
    }
}
