using Microsoft.AspNetCore.Mvc;
using RaoCattles.Modules.Users.Application.Services;
using RaoCattles.Modules.Users.Presentation.Requests;

namespace RaoCattles.Modules.Users.Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        try
        {
            var result = await authService.LoginAsync(request.Username, request.Password, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }
    }
}
