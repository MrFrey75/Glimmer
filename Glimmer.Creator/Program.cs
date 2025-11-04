using Glimmer.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Glimmer Core services (includes AuthenticationService with MongoDB infrastructure ready)
builder.Services.AddGlimmerCore(builder.Configuration);

// Add session support for authentication
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Note: Superuser is currently seeded in-memory by AuthenticationService constructor
// When services are refactored to use MongoDB, uncomment the following:
// using (var scope = app.Services.CreateScope())
// {
//     var authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
//     await authService.EnsureSuperUserExistsAsync();
// }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
