using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Glimmer.Creator.Models;

namespace Glimmer.Creator.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading home page");
            return RedirectToAction("Error");
        }
    }

    public IActionResult Privacy()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading privacy page");
            return RedirectToAction("Error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var statusCode = HttpContext.Response.StatusCode;
        var errorMessage = HttpContext.Items["ErrorMessage"]?.ToString();
        var errorDetails = HttpContext.Items["ErrorDetails"]?.ToString();

        var model = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = statusCode,
            Message = errorMessage ?? "An unexpected error occurred.",
            Details = errorDetails,
            Path = HttpContext.Request.Path
        };

        _logger.LogError("Error page displayed: Status {StatusCode}, Path: {Path}, Message: {Message}",
            statusCode, model.Path, model.Message);

        return View(model);
    }
}
