using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Glimmer.Creator.Models;
using Glimmer.Creator.Filters;

namespace Glimmer.Creator.Controllers;

[RequireAuthentication]
public class HomeController : BaseController
{
    public HomeController(ILogger<HomeController> logger) : base(logger)
    {
    }

    public IActionResult Index()
    {
        try
        {
            ViewBag.Username = GetCurrentUsername();
            return View();
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading home page", "Error");
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
            return HandleException(ex, "loading privacy page", "Error");
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

        Logger.LogError("Error page displayed: Status {StatusCode}, Path: {Path}, Message: {Message}",
            statusCode, model.Path, model.Message);

        return View(model);
    }
}
