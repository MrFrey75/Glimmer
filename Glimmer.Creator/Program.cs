using Glimmer.Core.Extensions;
using Glimmer.Core.Services;
using Serilog;
using Serilog.Events;

// Configure Serilog before creating the builder
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting Glimmer.Creator application");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithProperty("Application", "Glimmer.Creator"));

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    // Add Glimmer Core services (includes MongoDB repositories and services)
    builder.Services.AddGlimmerCore(builder.Configuration);

    // Add session support for authentication
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(60);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    var app = builder.Build();

    // Add Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.GetLevel = (httpContext, elapsed, ex) => ex != null
            ? LogEventLevel.Error
            : httpContext.Response.StatusCode > 499
                ? LogEventLevel.Error
                : LogEventLevel.Information;
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
            if (httpContext.User.Identity?.IsAuthenticated == true && httpContext.User.Identity.Name != null)
            {
                diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
            }
        };
    });

    // Ensure superuser exists in database
    using (var scope = app.Services.CreateScope())
    {
        var authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
        Log.Information("Ensuring superuser exists in database");
        await authService.EnsureSuperUserExistsAsync();
        Log.Information("Superuser check completed");
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    else
    {
        // Use custom exception middleware in development for better debugging
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseSession();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    Log.Information("Glimmer.Creator application started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Shutting down Glimmer.Creator application");
    Log.CloseAndFlush();
}
