using Glimmer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class AccountController : BaseController
{
    private readonly IAuthenticationService _authService;

    public AccountController(IAuthenticationService authService, ILogger<AccountController> logger) : base(logger)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string usernameOrEmail, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
        {
            Logger.LogWarning("Login attempt with missing credentials");
            ModelState.AddModelError(string.Empty, "Username/Email and password are required.");
            return View();
        }

        Logger.LogInformation("Login attempt for user: {UsernameOrEmail}", usernameOrEmail);
        
        try
        {
            var result = await _authService.LoginAsync(usernameOrEmail, password);

            if (result.Success)
            {
                SetAuthenticationCookieAndSession(
                    result.User!.Uuid.ToString(),
                    result.User.Username,
                    result.AccessToken!,
                    result.RefreshToken!,
                    result.ExpiresAt
                );

                Logger.LogInformation("User {Username} (ID: {UserId}) logged in successfully", 
                    result.User.Username, result.User.Uuid);

                TempData["SuccessMessage"] = $"Welcome back, {result.User.Username}!";
                return RedirectToAction("Index", "Home");
            }

            Logger.LogWarning("Failed login attempt for user: {UsernameOrEmail}. Reason: {Message}", 
                usernameOrEmail, result.Message);
            
            ModelState.AddModelError(string.Empty, result.Message ?? "Invalid credentials.");
            return View();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during login for user: {UsernameOrEmail}", usernameOrEmail);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string username, string email, string password, string confirmPassword, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || 
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            Logger.LogWarning("Registration attempt with missing required fields");
            ModelState.AddModelError(string.Empty, "All fields are required.");
            return View();
        }

        Logger.LogInformation("Registration attempt for username: {Username}, email: {Email}", username, email);
        
        try
        {
            if (!ValidatePasswordRequirements(password, confirmPassword, out var errorMessage))
            {
                Logger.LogWarning("Registration failed for {Username}: {ErrorMessage}", username, errorMessage);
                ModelState.AddModelError(string.Empty, errorMessage!);
                return View();
            }

            var result = await _authService.RegisterAsync(username, email, password);

            if (result.Success)
            {
                SetAuthenticationCookieAndSession(
                    result.User!.Uuid.ToString(),
                    result.User.Username,
                    result.AccessToken!,
                    result.RefreshToken!,
                    result.ExpiresAt
                );

                Logger.LogInformation("User {Username} (ID: {UserId}) registered successfully", 
                    result.User.Username, result.User.Uuid);

                TempData["SuccessMessage"] = $"Welcome to Glimmer, {result.User.Username}!";
                return RedirectToAction("Index", "Home");
            }

            Logger.LogWarning("Registration failed for {Username}: {Message}", username, result.Message);
            ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed.");
            return View();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during registration for username: {Username}, email: {Email}", username, email);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        var username = GetCurrentUsername();
        var userId = GetCurrentUserId();
        
        try
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                _ = _authService.RevokeTokenAsync(refreshToken, "User logout");
            }

            ClearAuthenticationCookieAndSession();

            Logger.LogInformation("User {Username} (ID: {UserId}) logged out", username ?? "Unknown", userId ?? "Unknown");

            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during logout for user {Username} (ID: {UserId})", username ?? "Unknown", userId ?? "Unknown");
            
            ClearAuthenticationCookieAndSession();
            
            return RedirectToAction("Login");
        }
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            Logger.LogWarning("Password reset request with missing email");
            ModelState.AddModelError(string.Empty, "Email address is required.");
            return View();
        }

        Logger.LogInformation("Password reset requested for email: {Email}", email);
        
        try
        {
            var token = await _authService.GeneratePasswordResetTokenAsync(email);

            TempData["SuccessMessage"] = "If that email address exists, password reset instructions have been sent.";
            Logger.LogInformation("Password reset token generation attempted for email: {Email}, Success: {Success}", 
                email, token != null);

            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during password reset request for email: {Email}", email);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            return View();
        }
    }

    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        ViewBag.Token = token;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(string token, string password, string confirmPassword, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(password) || 
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            Logger.LogWarning("Password reset attempt with missing required fields");
            ModelState.AddModelError(string.Empty, "All fields are required.");
            ViewBag.Token = token;
            return View();
        }

        Logger.LogInformation("Password reset attempt with token");
        
        try
        {
            if (!ValidatePasswordRequirements(password, confirmPassword, out var errorMessage))
            {
                Logger.LogWarning("Password reset failed: {ErrorMessage}", errorMessage);
                ModelState.AddModelError(string.Empty, errorMessage!);
                ViewBag.Token = token;
                return View();
            }

            var success = await _authService.ResetPasswordAsync(token, password);

            if (success)
            {
                TempData["SuccessMessage"] = "Password reset successfully. You can now log in with your new password.";
                Logger.LogInformation("Password reset successfully using token");
                return RedirectToAction("Login");
            }

            Logger.LogWarning("Password reset failed: Invalid or expired token");
            ModelState.AddModelError(string.Empty, "Invalid or expired reset token. Please request a new one.");
            ViewBag.Token = token;
            return View();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during password reset");
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            ViewBag.Token = token;
            return View();
        }
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserIdAsGuid();
            if (userId == null)
            {
                Logger.LogWarning("Change password attempt without authentication");
                TempData["ErrorMessage"] = "You must be logged in to change your password.";
                return RedirectToAction("Login");
            }

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword) || 
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                Logger.LogWarning("Change password attempt with missing required fields for user {UserId}", userId);
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return View();
            }

            if (currentPassword == newPassword)
            {
                Logger.LogWarning("Change password failed for user {UserId}: New password same as current", userId);
                ModelState.AddModelError(string.Empty, "New password must be different from current password.");
                return View();
            }

            if (!ValidatePasswordRequirements(newPassword, confirmPassword, out var errorMessage))
            {
                Logger.LogWarning("Change password failed for user {UserId}: {ErrorMessage}", userId, errorMessage);
                ModelState.AddModelError(string.Empty, errorMessage!);
                return View();
            }

            var success = await _authService.ChangePasswordAsync(userId.Value, currentPassword, newPassword);

            if (success)
            {
                TempData["SuccessMessage"] = "Password changed successfully.";
                Logger.LogInformation("Password changed successfully for user {UserId}", userId);
                return RedirectToAction("Index", "Home");
            }

            Logger.LogWarning("Change password failed for user {UserId}: Incorrect current password", userId);
            ModelState.AddModelError(string.Empty, "Current password is incorrect.");
            return View();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "password change");
        }
    }
}
