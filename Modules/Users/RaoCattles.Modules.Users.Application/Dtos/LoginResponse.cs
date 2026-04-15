namespace RaoCattles.Modules.Users.Application.Dtos;

public record LoginResponse(string Token, DateTime ExpiresAt);
