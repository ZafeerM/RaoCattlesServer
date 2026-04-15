using RaoCattles.Modules.Users.Domain.Entities;

namespace RaoCattles.Modules.Users.Application.Contracts;

public interface IJwtTokenService
{
    string GenerateToken(AdminUser user);
}
