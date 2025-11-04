using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public abstract class BaseController : Controller
{
    protected readonly ILogger Logger;

    protected BaseController(ILogger logger)
    {
        Logger = logger;
    }

    protected string? GetCurrentUserId() => HttpContext.Session.GetString("UserId");
    
    protected string? GetCurrentUsername() => HttpContext.Session.GetString("Username");
    
    protected Guid? GetCurrentUserIdAsGuid()
    {
        var userIdStr = GetCurrentUserId();
        return string.IsNullOrEmpty(userIdStr) ? null : Guid.Parse(userIdStr);
    }

    protected bool IsAuthenticated() => !string.IsNullOrEmpty(GetCurrentUserId());

    protected IActionResult RedirectToLogin()
    {
        TempData["ErrorMessage"] = "You must be logged in to access this page.";
        return RedirectToAction("Login", "Account");
    }

    protected IActionResult HandleException(Exception ex, string operation, string? redirectAction = null, string? redirectController = null)
    {
        Logger.LogError(ex, "Error during {Operation}", operation);
        
        if (!string.IsNullOrEmpty(redirectAction))
        {
            TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
            return RedirectToAction(redirectAction, redirectController);
        }
        
        ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
        return View();
    }

    protected bool ValidatePasswordRequirements(string password, string confirmPassword, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            errorMessage = "Password fields are required.";
            return false;
        }

        if (password != confirmPassword)
        {
            errorMessage = "Passwords do not match.";
            return false;
        }

        if (password.Length < 8)
        {
            errorMessage = "Password must be at least 8 characters long.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    protected void SetAuthenticationCookieAndSession(string userId, string username, string accessToken, string refreshToken, DateTime? expiresAt)
    {
        Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresAt
        });

        HttpContext.Session.SetString("UserId", userId);
        HttpContext.Session.SetString("Username", username);
        HttpContext.Session.SetString("AccessToken", accessToken);
    }

    protected void ClearAuthenticationCookieAndSession()
    {
        Response.Cookies.Delete("refresh_token");
        HttpContext.Session.Clear();
    }
}
