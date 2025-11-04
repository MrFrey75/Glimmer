using System.Net;
using System.Text.Json;

namespace Glimmer.Creator.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt for path: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized, "Unauthorized access.");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument for path: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "Invalid request data.");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found for path: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, "Resource not found.");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation for path: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "Invalid operation.");
        }
        catch (Exception ex)
        {
            var userId = context.Session.GetString("UserId");
            var username = context.Session.GetString("Username");
            
            _logger.LogError(ex, 
                "Unhandled exception for path: {Path}, User: {Username} (ID: {UserId})", 
                context.Request.Path, username ?? "Anonymous", userId ?? "N/A");
            
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, 
                "An unexpected error occurred. Please try again later.");
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context, 
        Exception exception, 
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;

        // If request expects JSON (API-like), return JSON response
        if (context.Request.Headers["Accept"].ToString().Contains("application/json"))
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Details = _environment.IsDevelopment() ? exception.Message : null,
                StackTrace = _environment.IsDevelopment() ? exception.StackTrace : null
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        else
        {
            // For MVC views, store error in TempData and redirect to Error page
            var errorDetails = _environment.IsDevelopment() 
                ? $"{message}\n\nDetails: {exception.Message}" 
                : message;

            context.Items["ErrorMessage"] = errorDetails;
            context.Items["StatusCode"] = statusCode;
            
            // Redirect to error page (handled by UseExceptionHandler in Program.cs)
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
