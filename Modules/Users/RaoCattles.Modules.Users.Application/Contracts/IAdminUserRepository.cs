using RaoCattles.Modules.Users.Domain.Entities;

namespace RaoCattles.Modules.Users.Application.Contracts;

public interface IAdminUserRepository
{
    Task<AdminUser?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task CreateAsync(AdminUser user, CancellationToken ct = default);
    Task<bool> AnyAsync(CancellationToken ct = default);
}
