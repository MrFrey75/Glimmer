# Glimmer - TODO List & Roadmap

**Last Updated**: 2025-11-04  
**Current Version**: 0.1.0-alpha  
**Status**: Early Development

## ✅ Completed (MVP Foundation)

### Core Infrastructure ✅
- [x] Project structure (2-tier architecture)
- [x] .NET 8 solution with Glimmer.Core and Glimmer.Creator
- [x] MongoDB integration with repository pattern
- [x] Dependency injection configuration
- [x] Service layer architecture

### Data Layer ✅
- [x] Domain models (BaseEntity, Universe, NotableFigure, Location, etc.)
- [x] MongoDB BSON attributes and serialization
- [x] Repository implementations:
  - [x] UserRepository (with unique indexes)
  - [x] TokenRepository (RefreshTokens, PasswordResetTokens)
  - [x] UniverseRepository (with user scoping)
  - [x] RelationRepository (with entity lookups)
- [x] Automatic index creation on startup
- [x] Soft delete pattern implementation

### Authentication & Security ✅
- [x] JWT token generation and validation
- [x] HMACSHA512 password hashing with salts
- [x] User registration and login
- [x] Refresh token system (7-day expiration)
- [x] Password reset workflow
- [x] Superuser seeding (Admin/Password1234)
- [x] AuthenticationService fully migrated to MongoDB

### Business Logic ✅
- [x] EntityService with 70+ methods for CRUD operations
- [x] Entity operations (Artifact, CannonEvent, Faction, Location, NotableFigure, Fact)
- [x] Relationship management system
- [x] Universe management
- [x] EntityService fully migrated to MongoDB

### Web Application ✅
- [x] ASP.NET Core MVC setup
- [x] Dark mode UI (always-on)
- [x] File ribbon navigation menu
- [x] Cascading submenu support
- [x] AccountController (Login, Register)
- [x] HomeController (basic pages)
- [x] Session management
- [x] HttpOnly cookie authentication

### Documentation ✅
- [x] Main README.md
- [x] Glimmer.Core README
- [x] Glimmer.Creator README
- [x] MongoDB migration documentation (OUTDATED)
- [x] Copilot instructions
- [x] This TODO.md file

---

## 🚧 In Progress

### Testing & Validation 🔄
- [ ] Manual testing of all authentication flows
  - [ ] Registration
  - [ ] Login/Logout
  - [ ] Password reset
  - [ ] Token refresh
- [ ] Manual testing of universe operations
  - [ ] Create universe
  - [ ] View universes
  - [ ] Edit universe
  - [ ] Delete universe
- [ ] Manual testing of entity operations
  - [ ] Add entities to universe
  - [ ] Edit entities
  - [ ] Delete entities
- [ ] Manual testing of relationship operations
  - [ ] Create relationships
  - [ ] View relationships
  - [ ] Delete relationships

---

## 📋 High Priority (Phase 1 - Core Functionality)

### Universe Management UI ✅
**Priority**: CRITICAL  
**Effort**: Medium  
**Status**: Complete ✅

- [x] UniverseController implementation
  - [x] GET /Universe - List all universes for current user
  - [x] GET /Universe/Create - Create universe form
  - [x] POST /Universe/Create - Handle creation
  - [x] GET /Universe/{id} - Universe details page
  - [x] GET /Universe/{id}/Edit - Edit universe form
  - [x] POST /Universe/{id}/Edit - Handle update
  - [x] POST /Universe/{id}/Delete - Handle deletion
- [x] Universe Views
  - [x] Index.cshtml - Universe list with cards
  - [x] Create.cshtml - Creation form
  - [x] Details.cshtml - Universe dashboard
  - [x] Edit.cshtml - Edit form
- [x] View Models
  - [x] UniverseListViewModel
  - [x] UniverseDetailsViewModel
  - [x] CreateUniverseViewModel
  - [x] EditUniverseViewModel
- [x] Navigation menu updated with Universe links

### Entity Management UI 🔴
**Priority**: CRITICAL  
**Effort**: Large  
**Status**: Not Started

#### NotableFigure (Characters) 🔴
- [ ] FigureController
  - [ ] GET /Universe/{id}/Figure/Create
  - [ ] POST /Universe/{id}/Figure/Create
  - [ ] GET /Universe/{id}/Figure/{figureId}
  - [ ] GET /Universe/{id}/Figure/{figureId}/Edit
  - [ ] POST /Universe/{id}/Figure/{figureId}/Edit
  - [ ] POST /Universe/{id}/Figure/{figureId}/Delete
- [ ] Views (Create, Edit, Details, List)
- [ ] View Models

#### Location 🔴
- [ ] LocationController (same pattern as Figure)
- [ ] Views
- [ ] View Models
- [ ] Hierarchical location support (parent locations)

#### Artifact 🟡
- [ ] ArtifactController
- [ ] Views
- [ ] View Models

#### CannonEvent 🟡
- [ ] CannonEventController
- [ ] Views
- [ ] View Models
- [ ] Timeline view

#### Faction 🟡
- [ ] FactionController
- [ ] Views
- [ ] View Models

#### Fact 🟡
- [ ] FactController
- [ ] Views
- [ ] View Models
- [ ] Tagging system

### Relationship Management UI 🟡
**Priority**: HIGH  
**Effort**: Large  
**Status**: Not Started

- [ ] RelationController
  - [ ] GET /Universe/{id}/Relations - List all relationships
  - [ ] GET /Universe/{id}/Relation/Create - Create form
  - [ ] POST /Universe/{id}/Relation/Create - Handle creation
  - [ ] GET /Universe/{id}/Relation/{relationId}/Edit
  - [ ] POST /Universe/{id}/Relation/{relationId}/Edit
  - [ ] POST /Universe/{id}/Relation/{relationId}/Delete
- [ ] Views
  - [ ] Index.cshtml - Relationship list
  - [ ] Create.cshtml - Creation form with entity pickers
  - [ ] Edit.cshtml - Edit form
- [ ] Entity picker component (select From/To entities)
- [ ] Relationship type dropdown (RelationTypeEnum)

---

## 📋 Medium Priority (Phase 2 - Enhanced UX)

### Search & Filter 🟡
**Priority**: MEDIUM  
**Effort**: Medium  
**Status**: Not Started

- [ ] Global search across all entities
- [ ] Filter by entity type
- [ ] Filter by creation date
- [ ] Search within universe
- [ ] Advanced search with multiple criteria
- [ ] Search results page
- [ ] Autocomplete search suggestions

### Visualization 🟡
**Priority**: MEDIUM  
**Effort**: Large  
**Status**: Not Started

- [ ] Relationship graph visualization (vis.js or D3.js)
  - [ ] Interactive node-link diagram
  - [ ] Zoom and pan
  - [ ] Filter by relationship type
  - [ ] Click to view entity details
- [ ] Timeline view for CannonEvents
  - [ ] Chronological ordering
  - [ ] Filter by date range
  - [ ] Interactive timeline component
- [ ] Map view for Locations
  - [ ] Hierarchical location tree
  - [ ] Visual map representation (future)

### User Experience Improvements 🟡
**Priority**: MEDIUM  
**Effort**: Small-Medium  
**Status**: Not Started

- [ ] Breadcrumb navigation
- [ ] Pagination for entity lists
- [ ] Sorting options (name, date, type)
- [ ] Bulk operations (delete, move)
- [ ] Confirmation modals for deletions
- [ ] Toast notifications for success/error
- [ ] Loading spinners for async operations
- [ ] Form validation improvements
- [ ] Rich text editor for descriptions (TinyMCE or Quill)
- [ ] Character/word count for descriptions
- [ ] Auto-save drafts

### Account Management 🟡
**Priority**: MEDIUM  
**Effort**: Small  
**Status**: Not Started

- [ ] User profile page
  - [ ] View profile
  - [ ] Edit profile (username, email)
  - [ ] Change password
  - [ ] Delete account
- [ ] Account settings page
  - [ ] UI preferences
  - [ ] Notification settings
  - [ ] Privacy settings
- [ ] Email verification flow
- [ ] Account activation/deactivation

---

## 📋 Lower Priority (Phase 3 - Advanced Features)

### Data Export/Import 🔵
**Priority**: LOW  
**Effort**: Medium  
**Status**: Not Started

- [ ] Export universe to JSON
- [ ] Export universe to PDF (report format)
- [ ] Export universe to Markdown
- [ ] Import universe from JSON
- [ ] Import from CSV (for entities)
- [ ] Backup/restore functionality
- [ ] Export relationship graph as image

### Templates & Presets 🔵
**Priority**: LOW  
**Effort**: Medium  
**Status**: Not Started

- [ ] Universe templates (Fantasy, Sci-Fi, Modern, etc.)
- [ ] Character templates
- [ ] Location templates
- [ ] Predefined relationship types for genres
- [ ] Template marketplace (future)

### Collaboration Features 🔵
**Priority**: LOW  
**Effort**: Large  
**Status**: Not Started

- [ ] Share universe with other users (read-only)
- [ ] Collaborative editing (multiple users)
- [ ] Comments on entities
- [ ] Activity feed/timeline
- [ ] Real-time updates with SignalR
- [ ] User roles (Owner, Editor, Viewer)

### Advanced Search & Analytics 🔵
**Priority**: LOW  
**Effort**: Medium  
**Status**: Not Started

- [ ] Full-text search with MongoDB Atlas Search
- [ ] Entity relationship analytics
  - [ ] Most connected entities
  - [ ] Relationship statistics
  - [ ] Entity count by type
- [ ] Universe statistics dashboardAPI
  - [ ] Total entities
  - [ ] Total relationships
  - [ ] Growth over time charts
- [ ] Tag cloud visualization
- [ ] Saved searches

### Mobile & API 🔵
**Priority**: LOW  
**Effort**: Large  
**Status**: Not Started

- [ ] Responsive mobile improvements
- [ ] Progressive Web App (PWA) support
- [ ] RESTful API for external access
  - [ ] API authentication (API keys)
  - [ ] API documentation (Swagger/OpenAPI)
  - [ ] Rate limiting
  - [ ] API versioning
- [ ] Mobile app (React Native or MAUI)

### AI & Content Generation 🔵
**Priority**: LOW  
**Effort**: Large  
**Status**: Not Started

- [ ] AI-powered description suggestions
- [ ] AI-generated character names
- [ ] AI-generated backstories
- [ ] Consistency checking (detect contradictions)
- [ ] Relationship suggestions based on existing entities
- [ ] OpenAI GPT integration

---

## 🔧 Technical Improvements

### Testing 🟡
**Priority**: HIGH  
**Effort**: Large  
**Status**: Not Started

- [ ] Unit tests for services
  - [ ] AuthenticationService tests
  - [ ] EntityService tests
- [ ] Unit tests for repositories
  - [ ] UserRepository tests
  - [ ] TokenRepository tests
  - [ ] UniverseRepository tests
  - [ ] RelationRepository tests
- [ ] Integration tests
  - [ ] Controller tests
  - [ ] End-to-end scenarios
- [ ] Test coverage target: 80%+
- [ ] Automated test runs in CI/CD

### Code Quality ✅
**Priority**: MEDIUM  
**Effort**: Small  
**Status**: Complete ✅

- [x] Serilog logging implementation with structured logging
- [x] User context enrichment in log entries
- [x] MongoDB, File, and Console logging sinks
- [x] Request logging middleware
- [x] BaseController with shared helper methods
- [x] Consolidated duplicate code in controllers
- [x] Global error handling middleware with friendly error pages

### Performance Optimization 🔵
**Priority**: LOW  
**Effort**: Medium  
**Status**: Not Started

- [ ] Response caching
- [ ] MongoDB query optimization
  - [ ] Index usage analysis
  - [ ] Query profiling
  - [ ] Projection optimization
- [ ] Entity pagination (avoid loading all at once)
- [ ] Lazy loading for relationships
- [ ] CDN for static assets
- [ ] Minification and bundling (CSS/JS)
- [ ] Image optimization

### Security Enhancements 🟡
**Priority**: MEDIUM  
**Effort**: Medium  
**Status**: Not Started

- [ ] Security audit
- [ ] OWASP Top 10 compliance check
- [ ] Rate limiting for authentication endpoints
- [ ] CAPTCHA for registration
- [ ] Two-factor authentication (2FA)
- [ ] Account lockout after failed attempts
- [ ] Security headers (CSP, HSTS, X-Frame-Options)
- [ ] Input sanitization and validation
- [ ] SQL/NoSQL injection prevention review
- [ ] XSS prevention review
- [ ] CSRF token validation

### DevOps & Deployment 🔵
**Priority**: LOW  
**Effort**: Medium  
**Status**: Not Started

- [ ] Docker Compose setup (app + MongoDB)
- [ ] Dockerfile for application
- [ ] CI/CD pipeline (GitHub Actions)
  - [ ] Automated builds
  - [ ] Automated tests
  - [ ] Automated deployments
- [ ] Environment configurations (Dev, Staging, Prod)
- [ ] Kubernetes deployment (optional)
- [ ] Azure/AWS deployment scripts
- [ ] Monitoring and logging (Application Insights, Seq)
- [ ] Health check endpoints

---

## 📚 Documentation Updates Needed

### Update Existing Docs 🟡
- [x] ~~Update `MONGODB_MIGRATION.md` - Mark as OUTDATED or delete~~ - Removed
- [x] ~~Update `Glimmer.Core/README.md` - Reflect MongoDB completion~~ - Updated
- [x] ~~Update `Glimmer.Creator/README.md` - Add new controllers~~ - Updated
- [x] ~~Update `.github/copilot-instructions.md` - Add new patterns~~ - Updated
- [x] ~~Update `README.md` - Keep in sync with features~~ - Updated
- [x] ~~Update `QUICK_REFERENCE.md` - Remove outdated references~~ - Updated
- [x] ~~Update `TODO.md` - Mark completed tasks~~ - Updated

### New Documentation 🔵
- [ ] API documentation (if API is built)
- [ ] Deployment guide
- [ ] Contributing guide
- [ ] Code of conduct
- [ ] Security policy (SECURITY.md)
- [ ] Changelog (CHANGELOG.md)
- [ ] User manual/guide
- [ ] Video tutorials

---

## 🐛 Known Issues

### Critical Bugs 🔴
- None currently identified

### Minor Issues 🟡
- None currently identified

### Technical Debt 🔵
- None currently identified

---

## 📊 Project Metrics

### Code Statistics (Estimated)
- **Total Lines of Code**: ~10,000
- **Core Layer**: ~5,000 lines
- **Web Layer**: ~3,000 lines
- **Views**: ~2,000 lines
- **Test Coverage**: 0% (needs improvement)

### Completion Status
- **Core Infrastructure**: 100% ✅
- **Authentication**: 100% ✅
- **Data Layer**: 100% ✅
- **Business Logic**: 100% ✅
- **Logging & Error Handling**: 100% ✅
- **Web UI**: 35% 🚧
- **Testing**: 0% ❌

### Overall Project Completion: ~50%

---

## 🎯 Milestones

### Milestone 1: MVP (Weeks 1-4) - 85% Complete ✅
- [x] Project setup
- [x] MongoDB integration
- [x] Authentication system
- [x] Basic dark mode UI
- [x] Logging and error handling
- [x] Universe CRUD UI
- [ ] Entity CRUD UI (at least 1 type)

### Milestone 2: Core Features (Weeks 5-8) - 0% Complete 🚧
- [ ] All entity types UI
- [ ] Relationship management UI
- [ ] Search functionality
- [ ] Basic visualization
- [ ] User profile management

### Milestone 3: Enhanced UX (Weeks 9-12) - 0% Complete ⏳
- [ ] Advanced search
- [ ] Relationship graph visualization
- [ ] Timeline view
- [ ] Export/import
- [ ] Rich text editing

### Milestone 4: Polish & Launch (Weeks 13-16) - 0% Complete ⏳
- [ ] Testing (80% coverage)
- [ ] Performance optimization
- [ ] Security audit
- [ ] Documentation complete
- [ ] Production deployment

---

## 🔄 Sprint Planning

### Current Sprint (Sprint 1)
**Focus**: Universe Management UI  
**Duration**: 2 weeks  
**Goal**: Implement full CRUD UI for universes
**Status**: ✅ Complete

Tasks:
1. ✅ Create UniverseController with all actions
2. ✅ Create Universe views (Index, Create, Edit, Details)
3. ✅ Create view models
4. ⏳ Test all universe operations
5. ⏳ Add validation and error handling

### Next Sprint (Sprint 2)
**Focus**: NotableFigure Management UI  
**Duration**: 2 weeks  
**Goal**: Implement full CRUD UI for characters

---

## 📞 Questions & Decisions Needed

### Architecture Decisions
- [ ] Should we implement CQRS pattern for read/write separation?
- [ ] Do we need real-time features (SignalR)?
- [ ] API-first approach for future mobile app?

### Design Decisions
- [ ] Image upload for entities?
- [ ] Markdown vs rich text editor?
- [ ] Dark mode toggle or always-on?

### Feature Priorities
- [ ] Which entity type should we implement first after Universe?
- [ ] Should relationship visualization come before all entity types?
- [ ] Do we need export before advanced features?

---

## 🎉 Future Vision (v2.0+)

- **AI Integration**: GPT-powered content generation
- **Collaboration**: Real-time multi-user editing
- **Mobile Apps**: Native iOS and Android apps
- **Marketplace**: Template and asset sharing
- **Plugins**: Extensibility system for custom features
- **Integrations**: Export to Obsidian, Notion, World Anvil
- **API Ecosystem**: Public API for third-party tools
- **Multi-language**: Internationalization support
- **Advanced Visualizations**: 3D maps, interactive timelines
- **Voice Input**: Dictation for entity creation

---

**Legend**:
- 🔴 Critical/High Priority
- 🟡 Medium Priority
- 🔵 Low Priority
- ✅ Completed
- 🚧 In Progress
- ⏳ Planned
- ❌ Blocked/Not Started
API