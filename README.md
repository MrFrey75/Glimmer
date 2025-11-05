# Glimmer - Universe Building Tool

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-purple.svg)](https://docs.microsoft.com/en-us/aspnet/core/)
[![MongoDB](https://img.shields.io/badge/MongoDB-2.22.0-green.svg)](https://www.mongodb.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Glimmer is a comprehensive universe building tool designed for writers, storytellers, and world builders. Create rich, interconnected universes with detailed characters, locations, events, and relationships - all powered by MongoDB for persistent storage.

## 🏗️ Architecture

Glimmer follows a clean 2-tier architecture:

- **[Glimmer.Core](Glimmer.Core/README.md)**: Domain models, business logic, services, and MongoDB repositories (.NET 8 class library)
- **[Glimmer.Creator](Glimmer.Creator/README.md)**: ASP.NET Core MVC web application with dark mode UI (.NET 8 MVC)

```
┌─────────────────────────────────────┐
│     Web Browser (Dark Mode UI)     │
└────────────────┬────────────────────┘
                 │
┌────────────────▼────────────────────┐
│    Glimmer.Creator (MVC Layer)     │
│  Controllers │ Views │ wwwroot     │
└────────────────┬────────────────────┘
                 │
┌────────────────▼────────────────────┐
│     Glimmer.Core (Domain Layer)    │
│  Services │ Models │ Repositories  │
└────────────────┬────────────────────┘
                 │
┌────────────────▼────────────────────┐
│      MongoDB (Persistence)         │
│  GlimmerDB │ Collections │ Indexes │
└─────────────────────────────────────┘
```

## 🌟 Features

### Domain Modeling
- **Universe Management**: Create and manage multiple universes with full CRUD operations
- **7 Entity Types**: NotableFigures (19 types), Locations (hierarchy), Artifacts (19 types), CannonEvents (20 types), Factions (13 types), Facts (11 types), Species (16 types)
- **101 Type Variants**: Rich categorization across all entity types for detailed world-building
- **Relationship System**: Rich semantic relationships between any entities (ParentOf, LocatedIn, AllyOf, etc.)
- **Hierarchical Locations**: Parent-child relationships for regions, countries, cities, buildings
- **Soft Delete**: Safe entity removal with recovery options
- **Embedded Collections**: Entities stored within universes for efficient queries

### Authentication & Security
- **JWT-based Authentication**: Secure access and refresh token system
- **User Management**: Registration, login, password reset, email verification
- **HMACSHA512 Encryption**: Industry-standard password hashing with salts
- **Superuser System**: Admin account (Admin/Password1234) seeded on startup
- **Session Management**: Secure HttpOnly cookies for token storage

### Data Persistence (MongoDB)
- **MongoDB Repositories**: Full repository pattern implementation
- **GUID Primary Keys**: Distributed system-friendly identifiers mapped to MongoDB _id
- **Indexed Collections**: Optimized queries with automatic index creation
- **Async/Await**: All database operations are asynchronous for better performance
- **Embedded Documents**: Universes contain entities for efficient single-query retrieval

### Logging & Monitoring
- **Serilog Integration**: Structured logging with multiple sinks
  - Console logging for development
  - File logging with daily rotation (30-day retention)
  - MongoDB logging for persistent audit trails
- **User Context Tracking**: All operations include user identification
- **Request Logging**: Automatic HTTP request/response logging
- **Log Enrichment**: Machine name, thread ID, environment info
- **Configurable Log Levels**: Per-component log level control

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK ([Download](https://dotnet.microsoft.com/download))
- MongoDB 7.0+ ([Docker](#mongodb-setup) or [Local Installation](#mongodb-setup))
- Visual Studio 2022, VS Code, or JetBrains Rider (optional)

### MongoDB Setup

#### Option 1: Docker (Recommended)
```bash
# Start MongoDB container
docker run -d -p 27017:27017 --name mongodb mongo:latest

# Verify it's running
docker ps | grep mongodb
```

#### Option 2: Local Installation
```bash
# Ubuntu/Debian
sudo apt-get install mongodb-org
sudo systemctl start mongod

# macOS
brew install mongodb-community
brew services start mongodb-community

# Windows
# Download installer from https://www.mongodb.com/try/download/community
```

### Building and Running

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

The application will be available at **http://localhost:5228**

### First Login

1. Navigate to http://localhost:5228
2. Click "Login" in the menu
3. Use default superuser credentials:
   - **Username**: `Admin`
   - **Password**: `Password1234`
4. **Important**: Change the password immediately after first login!

### Configuration

Update `Glimmer.Creator/appsettings.json` if needed:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "GlimmerDB"
  },
  "Jwt": {
    "Secret": "GlimmerCreator-SecretKey-ChangeInProduction-MinimumLength32Chars!",
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
│   ├── Models/                # Domain entities with MongoDB attributes
│   │   ├── BaseEntity.cs      # Common entity properties ([BsonId])
│   │   ├── Universe.cs        # Root aggregate
│   │   ├── NotableFigure.cs   # Characters/people (19 types)
│   │   ├── Location.cs        # Places with hierarchy (11 types)
│   │   ├── Artifact.cs        # Objects and items (19 types)
│   │   ├── CannonEvent.cs     # Historical events (20 types)
│   │   ├── Faction.cs         # Groups and organizations (13 types)
│   │   ├── Fact.cs           # Lore and trivia (11 types)
│   │   ├── Species.cs         # Lifeforms and creatures (16 types)
│   │   ├── EntityRelation.cs  # Relationship modeling
│   │   ├── User.cs           # User accounts
│   │   ├── RefreshToken.cs    # JWT refresh tokens
│   │   └── PasswordResetToken.cs # Password reset tokens
│   ├── Enums/                # Domain enumerations (101 type variants)
│   │   ├── RelationTypeEnum.cs # Relationship types
│   │   ├── FigureTypeEnum.cs  # 19 character types
│   │   ├── LocationTypeEnum.cs # 11 location types
│   │   ├── ArtifactTypeEnum.cs # 19 artifact types
│   │   ├── CannonEventTypeEnum.cs # 20 event types
│   │   ├── FactionTypeEnum.cs # 13 faction types
│   │   ├── FactTypeEnum.cs    # 11 fact types
│   │   └── SpeciesTypeEnum.cs # 16 species types
│   ├── Services/             # Business services (Modular architecture)
│   │   ├── IEntityService.cs  # Service interface
│   │   ├── EntityService.cs   # Main DI class
│   │   ├── EntityService.*.cs # 10 partial classes by entity type
│   │   └── AuthenticationService.cs # User auth & JWT (MongoDB)
│   ├── Repositories/         # MongoDB data access layer
│   │   ├── UserRepository.cs # User CRUD with unique indexes
│   │   ├── TokenRepository.cs # Token management
│   │   ├── UniverseRepository.cs # Universe CRUD
│   │   └── RelationRepository.cs # Relationship CRUD
│   ├── Configuration/        # Settings models
│   │   ├── JwtSettings.cs    # JWT configuration
│   │   └── MongoDbSettings.cs # MongoDB configuration
│   └── Extensions/           # DI extensions
│       └── ServiceCollectionExtensions.cs # Service registration
├── Glimmer.Creator/          # Web application layer → [README](Glimmer.Creator/README.md)
│   ├── Controllers/          # MVC controllers
│   │   ├── BaseController.cs  # Shared controller functionality
│   │   ├── HomeController.cs  # Main application & dashboard
│   │   ├── AccountController.cs # Authentication
│   │   ├── UniverseController.cs # Universe CRUD
│   │   ├── NotableFigureController.cs # Character CRUD
│   │   ├── LocationController.cs # Location CRUD (hierarchy)
│   │   ├── ArtifactController.cs # Artifact CRUD
│   │   ├── CannonEventController.cs # Event CRUD
│   │   ├── FactionController.cs # Faction CRUD
│   │   ├── FactController.cs # Fact CRUD
│   │   └── SpeciesController.cs # Species CRUD
│   ├── Views/               # Razor views (Dark mode)
│   │   ├── Home/           # Dashboard and main views
│   │   ├── Account/        # Auth views (Login, Register)
│   │   ├── Universe/       # Universe management
│   │   ├── NotableFigure/  # Character management
│   │   ├── Location/       # Location management
│   │   ├── Artifact/       # Artifact management
│   │   ├── CannonEvent/    # Event management
│   │   ├── Faction/        # Faction management
│   │   ├── Fact/           # Fact management
│   │   ├── Species/        # Species management
│   │   └── Shared/         # Layouts (_Layout.cshtml, _FileRibbon.cshtml)
│   ├── wwwroot/             # Static assets
│   │   ├── css/            # Dark mode styles (site.css)
│   │   ├── js/             # JavaScript (site.js)
│   │   └── lib/            # Bootstrap, jQuery
│   ├── Program.cs          # App startup & superuser seeding
│   └── appsettings.json    # Configuration
├── .github/                 # GitHub configuration
│   └── copilot-instructions.md # AI coding guidelines
└── README.md               # This file
```

## 🎯 Core Concepts

### Entities and Relationships
All domain entities inherit from `BaseEntity` providing:
```csharp
[BsonId]
[BsonElement("_id")]
public Guid Uuid { get; set; } = Guid.NewGuid();
public required string Name { get; set; }
public required string Description { get; set; }
public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
public bool IsDeleted { get; set; } = false;
```

### Relationship System
Entities connect via `EntityRelation` with semantic `RelationTypeEnum`:
- **Spatial**: LocatedIn, OccurredAt, Contains
- **Ownership**: CreatedBy, OwnedBy, DiscoveredBy
- **Social**: ParentOf, ChildOf, AllyOf, EnemyOf, SpouseOf, SiblingOf
- **Organizational**: MemberOf, LeaderOf, RuledOver
- **Historical**: ParticipatedIn, InfluencedBy, ContemporaryOf

### Authentication Flow
1. User registration with email verification (optional)
2. JWT access token (60 min) + refresh token (7 days)
3. Tokens stored in HttpOnly cookies for security
4. Automatic token refresh on expiration
5. Secure password reset via time-limited tokens

### MongoDB Integration
- **Collections**: users, universes, relations, refreshTokens, passwordResetTokens
- **Indexes**: Automatic creation on startup for optimal query performance
- **Embedded Documents**: Entities stored within universes (one-to-many)
- **Separate Collections**: Users, relations, and tokens stored separately
- **BSON Mapping**: Guid properties map to MongoDB _id field

## 🔧 Development

### Branching Strategy

**⚠️ IMPORTANT: Never develop directly on the `main` branch!**

The project follows a simple branching workflow:

- **`main`** - Production-ready code only. Protected branch for stable releases.
- **`development`** - Active development branch. All feature work happens here.
- **`feature/*`** - Feature branches (optional) for larger changes, merge to `development`.

#### Development Workflow
```bash
# Always work in the development branch
git checkout development

# For larger features, create a feature branch
git checkout -b feature/my-feature

# When complete, merge back to development
git checkout development
git merge feature/my-feature

# Main branch is updated only through pull requests from development
```

### Running with Hot Reload
```bash
cd Glimmer.Creator
dotnet watch run
```

### Database Management

#### View Database Contents
```bash
# Connect to MongoDB shell
docker exec -it mongodb mongosh

# Switch to Glimmer database
use GlimmerDB

# View collections
show collections

# Query users
db.users.find().pretty()

# Query universes
db.universes.find().pretty()

# Count documents
db.users.countDocuments()
```

#### Clear Database
```bash
# Drop entire database (WARNING: deletes all data!)
docker exec mongodb mongosh GlimmerDB --eval "db.dropDatabase()"

# Drop specific collection
docker exec mongodb mongosh GlimmerDB --eval "db.users.drop()"
```

### Running Tests
```bash
dotnet test
```

### Code Quality
```bash
# Format code
dotnet format

# Analyze code
dotnet build /p:TreatWarningsAsErrors=true
```

## 📖 Documentation

### Main Documentation
- **[README.md](README.md)** - Project overview and setup (this file)
- **[TODO.md](TODO.md)** - Complete project roadmap and task list
- **[QUICK_REFERENCE.md](QUICK_REFERENCE.md)** - Quick start and common commands

### Component Documentation
- **[Glimmer.Core README](Glimmer.Core/README.md)** - Domain layer, services, and repositories
- **[Glimmer.Core Services README](Glimmer.Core/Services/README.md)** - EntityService modular architecture
- **[Glimmer.Creator README](Glimmer.Creator/README.md)** - Web application and UI

### Development Guidelines
- **[Copilot Instructions](.github/copilot-instructions.md)** - AI coding guidelines and patterns

## 🛡️ Security Considerations

### Production Checklist
- [ ] Change default superuser password
- [ ] Update JWT secret in `appsettings.json` (use environment variables)
- [ ] Enable MongoDB authentication
- [ ] Configure HTTPS with valid SSL certificate
- [ ] Set up CORS policies
- [ ] Implement rate limiting
- [ ] Enable application logging (Application Insights, Seq, etc.)
- [ ] Configure secure cookie settings
- [ ] Set up backup strategy for MongoDB
- [ ] Implement security headers (CSP, HSTS, etc.)

### Security Features
- HMACSHA512 password hashing with unique salts
- JWT tokens with configurable expiration
- HttpOnly cookies prevent XSS attacks
- Secure token generation using cryptographic RNG
- Superuser account cannot be deleted
- Soft delete for data recovery

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📋 TODO

See [TODO.md](TODO.md) for the complete project roadmap and task list.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with .NET 8 and ASP.NET Core MVC
- MongoDB for flexible, scalable data storage
- JWT for secure stateless authentication
- Bootstrap 5.3 for responsive UI
- Entity-relationship modeling inspired by domain-driven design principles

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/MrFrey75/Glimmer/issues)
- **Discussions**: [GitHub Discussions](https://github.com/MrFrey75/Glimmer/discussions)

---

**⭐ Star this repository if you find it helpful!**

