using MongoDB.Driver;
using RaoCattles.Modules.Users.Domain.Entities;

namespace RaoCattles.Modules.Users.Infrastructure.Seed;

public static class AdminSeeder
{
    public static async Task SeedAsync(IMongoDatabase database, string username, string password)
    {
        var collection = database.GetCollection<AdminUser>("admin_users");

        var exists = await collection.Find(u => u.Username == username).AnyAsync();
        if (exists)
            return;

        var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

        await collection.InsertOneAsync(new AdminUser
        {
            Username = username,
            PasswordHash = hash,
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        });
    }
}
