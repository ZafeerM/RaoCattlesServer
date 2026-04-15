using MongoDB.Driver;
using RaoCattles.Modules.Users.Application.Contracts;
using RaoCattles.Modules.Users.Domain.Entities;

namespace RaoCattles.Modules.Users.Infrastructure.Repositories;

public class AdminUserRepository(IMongoDatabase database) : IAdminUserRepository
{
    private readonly IMongoCollection<AdminUser> _collection =
        database.GetCollection<AdminUser>("admin_users");

    public async Task<AdminUser?> GetByUsernameAsync(string username, CancellationToken ct = default)
        => await _collection.Find(u => u.Username == username).FirstOrDefaultAsync(ct);

    public async Task CreateAsync(AdminUser user, CancellationToken ct = default)
        => await _collection.InsertOneAsync(user, cancellationToken: ct);

    public async Task<bool> AnyAsync(CancellationToken ct = default)
        => await _collection.Find(_ => true).AnyAsync(ct);
}
