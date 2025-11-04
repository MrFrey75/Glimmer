namespace Glimmer.Creator.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public string? Message { get; set; }
    public string? Details { get; set; }
    public int StatusCode { get; set; }
    public string? Path { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public bool ShowDetails => !string.IsNullOrEmpty(Details);
    
    public string ErrorTitle => StatusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        500 => "Internal Server Error",
        _ => "Error"
    };
}
