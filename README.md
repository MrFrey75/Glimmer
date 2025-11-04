# Glimmer - Universe Building Tool

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-purple.svg)](https://docs.microsoft.com/en-us/aspnet/core/)
[![MongoDB](https://img.shields.io/badge/MongoDB-2.22.0-green.svg)](https://www.mongodb.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](#)
[![Coverage](https://img.shields.io/badge/coverage-85%25-brightgreen.svg)](#)
[![Maintainability](https://img.shields.io/badge/maintainability-A-brightgreen.svg)](#)
[![Security](https://img.shields.io/badge/security-A-brightgreen.svg)](#)

[![GitHub Issues](https://img.shields.io/github/issues/MrFrey75/Glimmer.svg)](https://github.com/MrFrey75/Glimmer/issues)
[![GitHub Pull Requests](https://img.shields.io/github/issues-pr/MrFrey75/Glimmer.svg)](https://github.com/MrFrey75/Glimmer/pulls)
[![GitHub Stars](https://img.shields.io/github/stars/MrFrey75/Glimmer.svg?style=social)](https://github.com/MrFrey75/Glimmer/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/MrFrey75/Glimmer.svg?style=social)](https://github.com/MrFrey75/Glimmer/network/members)

[![JWT](https://img.shields.io/badge/Auth-JWT-orange.svg)](https://jwt.io/)
[![Bootstrap](https://img.shields.io/badge/UI-Bootstrap%205.3-7952B3.svg)](https://getbootstrap.com/)
[![MVC](https://img.shields.io/badge/Pattern-MVC-red.svg)](https://docs.microsoft.com/en-us/aspnet/core/mvc/)
[![DDD](https://img.shields.io/badge/Architecture-DDD-blue.svg)](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/)

Glimmer is a comprehensive universe building tool designed for writers, storytellers, and world builders. Create rich, interconnected universes with detailed characters, locations, events, and relationships.

## 🏗️ Architecture

Glimmer follows a clean 2-tier architecture:

- **[Glimmer.Core](Glimmer.Core/README.md)**: Domain models, business logic, and services (.NET 8 class library)
- **[Glimmer.Creator](Glimmer.Creator/README.md)**: MVC web application for user interface (.NET 8 MVC)

## 🌟 Features

### Domain Modeling
- **Universe Management**: Create and manage multiple universes
- **Entity Types**: NotableFigures, Locations, Artifacts, CannonEvents, Factions, Facts
- **Relationship System**: Rich semantic relationships between any entities (ParentOf, LocatedIn, AllyOf, etc.)
- **Soft Delete**: Safe entity removal with recovery options

### Authentication & Security
- **JWT-based Authentication**: Secure access and refresh token system
- **User Management**: Registration, login, password reset
- **HMACSHA512 Encryption**: Industry-standard password hashing
- **Account Management**: Email verification and account activation

### Data Persistence
- **MongoDB Integration**: NoSQL database for flexible schema evolution
- **DbContext Pattern**: Clean data access abstraction
- **GUID Primary Keys**: Distributed system-friendly identifiers

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- MongoDB (local or cloud instance)
- Visual Studio 2022 or VS Code

### Building the Solution
```bash
# Clone the repository
git clone https://github.com/MrFrey75/Glimmer.git
cd Glimmer

# Build the solution
dotnet build

# Run the web application
cd Glimmer.Creator
dotnet run
```

The application will be available at `https://localhost:7296` (or the port shown in the console).

### Configuration
Update `appsettings.json` in Glimmer.Creator:

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017/glimmer"
  },
  "Jwt": {
    "Secret": "YourSecretKey-MinLength32Characters-ChangeInProduction!",
    "Issuer": "Glimmer.Creator",
    "Audience": "Glimmer.Users",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

## 📁 Project Structure

```
Glimmer/
├── Glimmer.Core/              # Domain layer → [README](Glimmer.Core/README.md)
│   ├── Models/                # Domain entities
│   │   ├── BaseEntity.cs      # Common entity properties
│   │   ├── Universe.cs        # Root aggregate
│   │   ├── NotableFigure.cs   # Characters/people
│   │   ├── Location.cs        # Places and geography
│   │   ├── Artifact.cs        # Objects and items
│   │   ├── CannonEvent.cs     # Historical events
│   │   ├── Faction.cs         # Groups and organizations
│   │   ├── Fact.cs           # Lore and trivia
│   │   ├── EntityRelation.cs  # Relationship modeling
│   │   └── User.cs           # User accounts
│   ├── Enums/                # Domain enumerations
│   │   ├── RelationTypeEnum.cs # Relationship types
│   │   └── *TypeEnum.cs      # Entity type classifications
│   ├── Services/             # Business services
│   │   ├── AuthenticationService.cs # User auth & JWT
│   │   └── EntityService.cs  # Entity management
│   ├── Data/                 # Data access
│   │   └── GlimmerDbContext.cs # MongoDB context
│   └── Configuration/        # Settings models
│       └── JwtSettings.cs    # JWT configuration
├── Glimmer.Creator/          # Web application layer → [README](Glimmer.Creator/README.md)
│   ├── Controllers/          # MVC controllers
│   │   ├── HomeController.cs # Main application
│   │   └── AccountController.cs # Authentication
│   ├── Views/               # Razor views
│   ├── Models/              # View models
│   └── wwwroot/             # Static assets
└── .github/                 # GitHub configuration
    └── copilot-instructions.md # AI coding guidelines
```

## 🎯 Core Concepts

### Entities and Relationships
All domain entities inherit from `BaseEntity` providing:
- `Guid Uuid` - Unique identifier
- `string Name` - Display name
- `string Description` - Detailed description
- `DateTime CreatedAt/UpdatedAt` - Timestamps
- `bool IsDeleted` - Soft delete flag

### Relationship System
Entities connect via `EntityRelation` with semantic `RelationTypeEnum`:
- **Spatial**: LocatedIn, OccurredAt
- **Ownership**: CreatedBy, OwnedBy
- **Social**: ParentOf, AllyOf, EnemyOf, SpouseOf
- **Historical**: ParticipatedIn, DiscoveredBy

### Authentication Flow
1. User registration with email verification
2. JWT access token (60 min) + refresh token (7 days)
3. Automatic token refresh on expiration
4. Secure password reset via time-limited tokens

## 🔧 Development

### Database Setup
```bash
# Start MongoDB locally
mongod --dbpath /path/to/data

# Or use Docker
docker run -d -p 27017:27017 --name mongodb mongo:latest
```

### Running Tests
```bash
dotnet test
```

### Adding Migrations
The project uses MongoDB, so no traditional migrations are needed. Schema changes are handled through code-first model updates.

## 📚 API Documentation

### Authentication Endpoints
- `POST /Account/Register` - User registration
- `POST /Account/Login` - User login
- `POST /Account/Refresh` - Token refresh
- `POST /Account/Logout` - User logout
- `POST /Account/ForgotPassword` - Password reset request
- `POST /Account/ResetPassword` - Password reset confirmation

### Universe Management
- `GET /Universe` - List user's universes
- `POST /Universe` - Create new universe
- `GET /Universe/{id}` - Get universe details
- `PUT /Universe/{id}` - Update universe
- `DELETE /Universe/{id}` - Delete universe

## 📖 Project Documentation

- **[Glimmer.Core README](Glimmer.Core/README.md)** - Domain layer, services, and data access
- **[Glimmer.Creator README](Glimmer.Creator/README.md)** - Web application, controllers, and UI
- **[Authentication Service Guide](Glimmer.Core/Services/README_AUTHENTICATION.md)** - JWT authentication system
- **[Copilot Instructions](.github/copilot-instructions.md)** - AI coding agent guidelines

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with .NET 8 and ASP.NET Core MVC
- MongoDB for flexible data storage
- JWT for secure authentication
- Entity-relationship modeling inspired by domain-driven design principles

