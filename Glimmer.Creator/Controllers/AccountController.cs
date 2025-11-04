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
    public async Task<IActionResult> Login(string usernameOrEmail, string password)
    {
        try
        {
            var result = await _authService.LoginAsync(usernameOrEmail, password);

            if (result.Success)
            {
                // Store refresh token in HttpOnly cookie
                Response.Cookies.Append("refresh_token", result.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.ExpiresAt
                });

                // Store user info in session
                HttpContext.Session.SetString("UserId", result.User!.Uuid.ToString());
                HttpContext.Session.SetString("Username", result.User.Username);
                HttpContext.Session.SetString("AccessToken", result.AccessToken!);

                _logger.LogInformation($"User {result.User.Username} logged in successfully");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = result.Message;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            ViewBag.Error = "An error occurred during login. Please try again.";
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string username, string email, string password, string confirmPassword)
    {
        try
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            var result = await _authService.RegisterAsync(username, email, password);

            if (result.Success)
            {
                // Store refresh token in HttpOnly cookie
                Response.Cookies.Append("refresh_token", result.RefreshToken!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.ExpiresAt
                });

                // Store user info in session
                HttpContext.Session.SetString("UserId", result.User!.Uuid.ToString());
                HttpContext.Session.SetString("Username", result.User.Username);
                HttpContext.Session.SetString("AccessToken", result.AccessToken!);

                _logger.LogInformation($"User {result.User.Username} registered successfully");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = result.Message;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            ViewBag.Error = "An error occurred during registration. Please try again.";
            return View();
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            _ = _authService.RevokeTokenAsync(refreshToken, "User logout");
        }

        Response.Cookies.Delete("refresh_token");
        HttpContext.Session.Clear();

        _logger.LogInformation("User logged out");

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            var token = await _authService.GeneratePasswordResetTokenAsync(email);

            if (token != null)
            {
                // In production, send this token via email
                // For now, just show success message
                ViewBag.Success = "Password reset instructions have been sent to your email.";
                _logger.LogInformation($"Password reset token generated for email: {email}");
            }
            else
            {
                // Don't reveal if email exists for security
                ViewBag.Success = "If that email exists, password reset instructions have been sent.";
            }

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset request");
            ViewBag.Error = "An error occurred. Please try again.";
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
    public async Task<IActionResult> ResetPassword(string token, string password, string confirmPassword)
    {
        try
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                ViewBag.Token = token;
                return View();
            }

            var success = await _authService.ResetPasswordAsync(token, password);

            if (success)
            {
                ViewBag.Success = "Password reset successfully. You can now log in.";
                _logger.LogInformation("Password reset successfully");
                return View("Login");
            }

            ViewBag.Error = "Invalid or expired reset token.";
            ViewBag.Token = token;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            ViewBag.Error = "An error occurred. Please try again.";
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
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        try
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login");
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "New passwords do not match.";
                return View();
            }

            var userId = Guid.Parse(userIdStr);
            var success = await _authService.ChangePasswordAsync(userId, currentPassword, newPassword);

            if (success)
            {
                ViewBag.Success = "Password changed successfully.";
                _logger.LogInformation($"Password changed for user {userId}");
                return View();
            }

            ViewBag.Error = "Current password is incorrect.";
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password change");
            ViewBag.Error = "An error occurred. Please try again.";
            return View();
        }
    }
}
