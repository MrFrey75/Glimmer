# Glimmer.Creator - Web Application

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-purple.svg)](https://docs.microsoft.com/en-us/aspnet/core/mvc/)

The web application layer for Glimmer universe building tool. Provides a user-friendly web interface for creating and managing universes, characters, locations, and relationships.

## 🏗️ Architecture

Glimmer.Creator is built with ASP.NET Core MVC following the Model-View-Controller pattern:

```
Glimmer.Creator/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs     # Main application pages
│   └── AccountController.cs  # Authentication & user management
├── Views/               # Razor Views
│   ├── Home/                # Application views
│   ├── Account/             # Authentication views  
│   └── Shared/              # Shared layouts and partials
├── Models/              # View Models and DTOs
├── wwwroot/            # Static assets (CSS, JS, images)
│   ├── css/                # Stylesheets
│   ├── js/                 # JavaScript files
│   └── lib/                # Client-side libraries
└── Program.cs          # Application startup and configuration
```

## 🌟 Features

### Authentication & User Management
- **User Registration**: Create new user accounts with email verification
- **Secure Login**: JWT-based authentication with refresh tokens
- **Password Management**: Change password, forgot password flow
- **Session Management**: Secure session handling with HttpOnly cookies
- **Account Management**: Profile management and account settings

### Universe Building Interface
- **Dashboard**: Central hub for managing universes
- **Entity Management**: Create and edit characters, locations, artifacts, events
- **Relationship Visualization**: Interactive relationship mapping
- **Search & Filter**: Find entities and relationships quickly
- **Rich Text Editing**: Detailed descriptions with formatting support

### User Experience
- **Responsive Design**: Mobile-friendly interface
- **Real-time Updates**: Dynamic content updates
- **Intuitive Navigation**: Easy-to-use interface
- **Accessibility**: WCAG compliant design
- **Performance**: Optimized for fast loading

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- MongoDB (local or cloud)
- Visual Studio 2022 or VS Code (recommended)

### Configuration

Update `appsettings.json`:
```json
{
  "Jwt": {
    "Secret": "YourSecretKey-MinLength32Characters-ChangeInProduction!",
    "Issuer": "Glimmer.Creator",
    "Audience": "Glimmer.Users",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "GlimmerDB"
  }
}
```

### Running the Application

```bash
# Navigate to the Creator project
cd Glimmer.Creator

# Restore dependencies
dotnet restore

# Run the application
dotnet run

# Or for development with hot reload
dotnet watch run
```

The application will be available at:
- **HTTPS**: `https://localhost:7296`
- **HTTP**: `http://localhost:5152`

### Development Mode
```bash
# Run with development environment
ASPNETCORE_ENVIRONMENT=Development dotnet run

# Enable detailed errors and debugging
ASPNETCORE_DETAILEDERRORS=true dotnet run
```

## 🎯 Controllers

### HomeController
Main application controller handling:
- **Dashboard**: `/` - User's universe overview
- **Privacy**: `/Home/Privacy` - Privacy policy
- **Error Handling**: Global error pages

### AccountController  
Authentication controller managing:
- **Registration**: `GET/POST /Account/Register`
- **Login**: `GET/POST /Account/Login`
- **Logout**: `POST /Account/Logout`
- **Password Reset**: `GET/POST /Account/ForgotPassword`
- **Profile Management**: `GET/POST /Account/Profile`

**Key Features:**
```csharp
[HttpPost]
public async Task<IActionResult> Login(string usernameOrEmail, string password)
{
    var result = await _authService.LoginAsync(usernameOrEmail, password);
    
    if (result.Success)
    {
        // Set authentication cookies
        Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = result.ExpiresAt
        });
        
        return RedirectToAction("Index", "Home");
    }
    
    ModelState.AddModelError("", result.Message);
    return View();
}
```

## 🎨 Views & UI

### Layout Structure
- **_Layout.cshtml**: Main application layout with navigation
- **_LoginPartial.cshtml**: Authentication status and user menu
- **_ValidationScriptsPartial.cshtml**: Client-side validation

### View Features
- **Razor Pages**: Server-side rendering with C# integration
- **Bootstrap**: Responsive CSS framework
- **jQuery**: Client-side interactivity
- **Font Awesome**: Icon library
- **Custom CSS**: Application-specific styling

### Responsive Design
```css
/* Mobile-first responsive breakpoints */
@media (min-width: 576px) { /* Small devices */ }
@media (min-width: 768px) { /* Medium devices */ }
@media (min-width: 992px) { /* Large devices */ }
@media (min-width: 1200px) { /* Extra large devices */ }
```

## ⚙️ Configuration & Services

### Service Registration
```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// MVC Services
builder.Services.AddControllersWithViews();

// Glimmer Core Services (includes Authentication)
builder.Services.AddGlimmerCore();

// Session Support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

### Middleware Pipeline
```csharp
// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
```

## 🛡️ Security

### Authentication Flow
1. **Login**: User provides credentials
2. **Validation**: Server validates against database
3. **Token Generation**: JWT access token + refresh token created
4. **Cookie Storage**: Tokens stored in secure HttpOnly cookies
5. **Authorization**: Subsequent requests validated via JWT

### Security Headers
```csharp
// Security middleware (recommended additions)
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    await next();
});
```

### HTTPS Configuration
```csharp
// Force HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
```

## 📱 Client-Side Assets

### JavaScript Libraries
- **jQuery** (3.6.0) - DOM manipulation and AJAX
- **Bootstrap** (5.3.0) - UI framework and components
- **jQuery Validation** - Client-side form validation
- **Chart.js** - Data visualization (for universe analytics)
- **Custom JS** - Application-specific functionality

### CSS Framework
- **Bootstrap 5.3** - Responsive grid and components
- **Font Awesome** - Icon library
- **Custom Styles** - Application branding and specific styling

### Build Pipeline
```bash
# For production builds
dotnet publish -c Release -o ./publish

# Static asset optimization
dotnet add package BuildBundlerMinifier
```

## 🔧 Development

### Hot Reload
```bash
# Enable hot reload for development
dotnet watch run

# File watching for specific extensions
dotnet watch --project Glimmer.Creator run
```

### Debugging
```bash
# Run with debugger attached
dotnet run --configuration Debug

# Enable detailed debugging
export ASPNETCORE_DETAILEDERRORS=true
export ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

### Logging
```csharp
// Built-in logging in controllers
private readonly ILogger<HomeController> _logger;

_logger.LogInformation("User {UserId} accessed {Action}", userId, actionName);
_logger.LogWarning("Authentication failed for {Username}", username);
_logger.LogError(ex, "Error processing universe creation");
```

## 🧪 Testing

### Unit Testing
```bash
# Run tests
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Integration Testing
```csharp
// Example integration test
[Fact]
public async Task GET_Home_ReturnsSuccessAndCorrectContentType()
{
    var response = await _client.GetAsync("/");
    
    response.EnsureSuccessStatusCode();
    Assert.Equal("text/html; charset=utf-8", 
        response.Content.Headers.ContentType.ToString());
}
```

## 📦 Dependencies

### Core Dependencies
- **Glimmer.Core** - Domain layer reference
- **Microsoft.AspNetCore.Mvc** - MVC framework
- **Microsoft.Extensions.Logging** - Logging infrastructure

### Additional Packages
- **Bootstrap** (5.3.0) - UI framework
- **jQuery** (3.6.0) - JavaScript library
- **Font Awesome** (6.0.0) - Icons

## 🚀 Deployment

### Production Configuration
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "yourdomain.com",
  "Jwt": {
    "Secret": "ProductionSecretKey-StoreInEnvironmentVariables-VeryLongAndSecure!",
    "Issuer": "https://yourdomain.com",
    "Audience": "Glimmer.Users"
  },
  "MongoDB": {
    "ConnectionString": "mongodb://your-production-mongodb-connection",
    "DatabaseName": "GlimmerProduction"
  }
}
```

### Docker Support
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Glimmer.Creator/Glimmer.Creator.csproj", "Glimmer.Creator/"]
COPY ["Glimmer.Core/Glimmer.Core.csproj", "Glimmer.Core/"]
RUN dotnet restore "Glimmer.Creator/Glimmer.Creator.csproj"
```

## 🔄 Future Enhancements

- [ ] AI-powered content suggestions
- [ ] Advanced universe visualization
- [ ] Export/import functionality
- [ ] Mobile app companion
- [ ] Advanced search and filtering
- [ ] Universe templates and sharing
- [ ] Advanced role-based permissions
- [ ] API documentation with Swagger

## 📖 Additional Resources

- [ASP.NET Core MVC Documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/)
- [jQuery Documentation](https://api.jquery.com/)

---

**Note**: This is the web application layer of Glimmer. For the domain layer, see [Glimmer.Core README](../Glimmer.Core/README.md).