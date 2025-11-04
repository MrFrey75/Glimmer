using Glimmer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthenticationService authService, ILogger<AccountController> logger)
    {
        _authService = authService;
        _logger = logger;
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
            _logger.LogWarning("Login attempt with missing credentials");
            ModelState.AddModelError(string.Empty, "Username/Email and password are required.");
            return View();
        }

        _logger.LogInformation("Login attempt for user: {UsernameOrEmail}", usernameOrEmail);
        
        try
        {
            var result = await _authService.LoginAsync(usernameOrEmail, password);

            if (result.Success)
            {
                Response.Cookies.Append("refresh_token", result.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.ExpiresAt
                });

                HttpContext.Session.SetString("UserId", result.User!.Uuid.ToString());
                HttpContext.Session.SetString("Username", result.User.Username);
                HttpContext.Session.SetString("AccessToken", result.AccessToken!);

                _logger.LogInformation("User {Username} (ID: {UserId}) logged in successfully", 
                    result.User.Username, result.User.Uuid);

                TempData["SuccessMessage"] = $"Welcome back, {result.User.Username}!";
                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning("Failed login attempt for user: {UsernameOrEmail}. Reason: {Message}", 
                usernameOrEmail, result.Message);
            
            ModelState.AddModelError(string.Empty, result.Message ?? "Invalid credentials.");
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {UsernameOrEmail}", usernameOrEmail);
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
            _logger.LogWarning("Registration attempt with missing required fields");
            ModelState.AddModelError(string.Empty, "All fields are required.");
            return View();
        }

        _logger.LogInformation("Registration attempt for username: {Username}, email: {Email}", username, email);
        
        try
        {
            if (password != confirmPassword)
            {
                _logger.LogWarning("Registration failed for {Username}: Passwords do not match", username);
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View();
            }

            if (password.Length < 8)
            {
                _logger.LogWarning("Registration failed for {Username}: Password too short", username);
                ModelState.AddModelError(string.Empty, "Password must be at least 8 characters long.");
                return View();
            }

            var result = await _authService.RegisterAsync(username, email, password);

            if (result.Success)
            {
                Response.Cookies.Append("refresh_token", result.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.ExpiresAt
                });

                HttpContext.Session.SetString("UserId", result.User!.Uuid.ToString());
                HttpContext.Session.SetString("Username", result.User.Username);
                HttpContext.Session.SetString("AccessToken", result.AccessToken!);

                _logger.LogInformation("User {Username} (ID: {UserId}) registered successfully", 
                    result.User.Username, result.User.Uuid);

                TempData["SuccessMessage"] = $"Welcome to Glimmer, {result.User.Username}!";
                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning("Registration failed for {Username}: {Message}", username, result.Message);
            ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed.");
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for username: {Username}, email: {Email}", username, email);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        var username = HttpContext.Session.GetString("Username");
        var userId = HttpContext.Session.GetString("UserId");
        
        try
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                _ = _authService.RevokeTokenAsync(refreshToken, "User logout");
            }

            Response.Cookies.Delete("refresh_token");
            HttpContext.Session.Clear();

            _logger.LogInformation("User {Username} (ID: {UserId}) logged out", username ?? "Unknown", userId ?? "Unknown");

            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user {Username} (ID: {UserId})", username ?? "Unknown", userId ?? "Unknown");
            
            // Clear session anyway
            Response.Cookies.Delete("refresh_token");
            HttpContext.Session.Clear();
            
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
            _logger.LogWarning("Password reset request with missing email");
            ModelState.AddModelError(string.Empty, "Email address is required.");
            return View();
        }

        _logger.LogInformation("Password reset requested for email: {Email}", email);
        
        try
        {
            var token = await _authService.GeneratePasswordResetTokenAsync(email);

            // Always show success message for security (don't reveal if email exists)
            TempData["SuccessMessage"] = "If that email address exists, password reset instructions have been sent.";
            _logger.LogInformation("Password reset token generation attempted for email: {Email}, Success: {Success}", 
                email, token != null);

            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset request for email: {Email}", email);
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
            _logger.LogWarning("Password reset attempt with missing required fields");
            ModelState.AddModelError(string.Empty, "All fields are required.");
            ViewBag.Token = token;
            return View();
        }

        _logger.LogInformation("Password reset attempt with token");
        
        try
        {
            if (password != confirmPassword)
            {
                _logger.LogWarning("Password reset failed: Passwords do not match");
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                ViewBag.Token = token;
                return View();
            }

            if (password.Length < 8)
            {
                _logger.LogWarning("Password reset failed: Password too short");
                ModelState.AddModelError(string.Empty, "Password must be at least 8 characters long.");
                ViewBag.Token = token;
                return View();
            }

            var success = await _authService.ResetPasswordAsync(token, password);

            if (success)
            {
                TempData["SuccessMessage"] = "Password reset successfully. You can now log in with your new password.";
                _logger.LogInformation("Password reset successfully using token");
                return RedirectToAction("Login");
            }

            _logger.LogWarning("Password reset failed: Invalid or expired token");
            ModelState.AddModelError(string.Empty, "Invalid or expired reset token. Please request a new one.");
            ViewBag.Token = token;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            ViewBag.Token = token;
            return View();
        }
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword, CancellationToken cancellationToken = default)
    {
        try
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                _logger.LogWarning("Change password attempt without authentication");
                TempData["ErrorMessage"] = "You must be logged in to change your password.";
                return RedirectToAction("Login");
            }

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword) || 
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                _logger.LogWarning("Change password attempt with missing required fields for user {UserId}", userIdStr);
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return View();
            }

            if (currentPassword == newPassword)
            {
                _logger.LogWarning("Change password failed for user {UserId}: New password same as current", userIdStr);
                ModelState.AddModelError(string.Empty, "New password must be different from current password.");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                _logger.LogWarning("Change password failed for user {UserId}: Passwords do not match", userIdStr);
                ModelState.AddModelError(string.Empty, "New passwords do not match.");
                return View();
            }

            if (newPassword.Length < 8)
            {
                _logger.LogWarning("Change password failed for user {UserId}: Password too short", userIdStr);
                ModelState.AddModelError(string.Empty, "Password must be at least 8 characters long.");
                return View();
            }

            var userId = Guid.Parse(userIdStr);
            var success = await _authService.ChangePasswordAsync(userId, currentPassword, newPassword);

            if (success)
            {
                TempData["SuccessMessage"] = "Password changed successfully.";
                _logger.LogInformation("Password changed successfully for user {UserId}", userId);
                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning("Change password failed for user {UserId}: Incorrect current password", userId);
            ModelState.AddModelError(string.Empty, "Current password is incorrect.");
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password change");
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            return View();
        }
    }
}
