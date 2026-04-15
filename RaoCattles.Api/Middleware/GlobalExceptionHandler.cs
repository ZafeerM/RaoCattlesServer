using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RaoCattles.BuildingBlocks.Exceptions;

namespace RaoCattles.Api.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, title) = exception switch
        {
            NotFoundException         => (StatusCodes.Status404NotFound,            "Not Found"),
            ValidationException       => (StatusCodes.Status400BadRequest,           "Validation Error"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,       "Unauthorized"),
            _                         => (StatusCodes.Status500InternalServerError,  "An unexpected error occurred.")
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var problem = new ProblemDetails
        {
            Status = status,
            Title  = title,
            Detail = status == StatusCodes.Status500InternalServerError ? null : exception.Message
        };

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}
