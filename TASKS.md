# Glimmer - Immediate Task List

**Created**: 2025-11-04  
**Purpose**: Consolidated list of tasks identified during code review

---

## ✅ COMPLETED TASKS

### Logging System Implementation
- [x] Serilog integrated with Console, File, and MongoDB sinks
- [x] Structured logging with named properties
- [x] User context added to AuthenticationService logs
- [x] User context added to EntityService logs (Universe and Artifact operations)
- [x] Request logging middleware configured
- [x] Log enrichers added (MachineName, ThreadId, Environment)
- [x] Logging documentation created (README_LOGGING.md)

**Result**: Comprehensive logging system is now in place with user tracking capabilities for audit trails and debugging.

---

## 🔴 HIGH PRIORITY TASKS

### 1. Port Configuration Consistency ✅
**Status**: FIXED  
**Location**: Documentation  
**Issue**: README.md references port 5228 consistently  
**Resolution**: Application runs on port 5228 by default (https://localhost:5228)

### 2. Error Handling in Controllers
**Status**: NOT STARTED  
**Priority**: HIGH  
**Effort**: Medium

**Issues**:
- Controllers need consistent error handling patterns
- Need centralized exception handling middleware
- Error responses should be user-friendly and structured
- Need proper HTTP status codes (400, 404, 500, etc.)

**Tasks**:
- [ ] Create global exception handling middleware
- [ ] Add try-catch blocks to controller actions where missing
- [ ] Standardize error response format
- [ ] Add model validation error handling
- [ ] Implement ProblemDetails for API-style error responses
- [ ] Add user-friendly error messages in views
- [ ] Log all exceptions with proper context

**Files to Update**:
- `Glimmer.Creator/Controllers/AccountController.cs`
- `Glimmer.Creator/Controllers/HomeController.cs`
- Future controllers (Universe, Entity, etc.)
- Create: `Glimmer.Creator/Middleware/ExceptionHandlingMiddleware.cs`
- Create: `Glimmer.Creator/Models/ErrorViewModel.cs` (if not exists)

### 3. Validation and UX Improvements
**Status**: NOT STARTED  
**Priority**: HIGH  
**Effort**: Medium

**Issues**:
- Validation messages need better UX (toast notifications, inline errors)
- Form validation needs client-side support
- Success messages need consistent styling
- Loading indicators needed for async operations

**Tasks**:
- [ ] Add client-side validation (jQuery Unobtrusive Validation)
- [ ] Implement toast notification system (e.g., Toastr.js)
- [ ] Add loading spinners for async operations
- [ ] Create validation summary partial views
- [ ] Add field-level validation error display
- [ ] Implement success/error message flash system
- [ ] Add confirmation modals for destructive actions
- [ ] Improve validation error messages to be more user-friendly

**Files to Update**:
- `Glimmer.Creator/Views/Shared/_ValidationScriptsPartial.cshtml`
- `Glimmer.Creator/Views/Shared/_Layout.cshtml` (add toast container)
- `Glimmer.Creator/wwwroot/css/site.css` (validation styling)
- `Glimmer.Creator/wwwroot/js/site.js` (validation logic)
- All view files with forms

---

## 🟡 MEDIUM PRIORITY TASKS

### 4. Complete EntityService Logging
**Status**: PARTIALLY COMPLETE  
**Priority**: MEDIUM  
**Effort**: Small

**Current State**:
- ✅ Universe operations have logging
- ✅ Artifact create operation has logging
- ❌ Other entity operations need logging

**Remaining Tasks**:
- [ ] Add logging to CannonEvent operations (Create, Update, Delete)
- [ ] Add logging to Faction operations (Create, Update, Delete)
- [ ] Add logging to Location operations (Create, Update, Delete)
- [ ] Add logging to NotableFigure operations (Create, Update, Delete)
- [ ] Add logging to Fact operations (Create, Update, Delete)
- [ ] Add logging to EntityRelation operations (Create, Update, Delete)

**Pattern to Follow**:
```csharp
_logger.LogInformation("Creating {EntityType} '{Name}' in universe {UniverseId}", 
    "NotableFigure", name, universeId);
// ... operation ...
_logger.LogInformation("{EntityType} {EntityId} '{Name}' created in universe {UniverseId}", 
    "NotableFigure", figure.Uuid, name, universeId);
```

### 5. MongoDB Connection Error Handling
**Status**: NOT STARTED  
**Priority**: MEDIUM  
**Effort**: Small

**Issue**: Application fails silently if MongoDB is not running

**Tasks**:
- [ ] Add MongoDB health check on startup
- [ ] Display user-friendly error message if MongoDB is unavailable
- [ ] Add retry logic for transient MongoDB connection failures
- [ ] Log MongoDB connection errors with helpful messages
- [ ] Create troubleshooting guide in documentation

**Files to Update**:
- `Glimmer.Creator/Program.cs` (add health check)
- `README.md` (add troubleshooting section)

### 6. Complete User Context in Logs
**Status**: PARTIALLY COMPLETE  
**Priority**: MEDIUM  
**Effort**: Small

**Current State**:
- ✅ Username and UserId included in AuthenticationService logs
- ✅ Username and UserId included in key EntityService operations
- ❌ Request logging middleware captures authenticated username
- ❌ But not all operations include user context where relevant

**Tasks**:
- [ ] Review all EntityService methods and add user context where appropriate
- [ ] Add user context to controller logs
- [ ] Consider adding LogContext.PushProperty for user info in middleware
- [ ] Document logging patterns for future developers

**Example Enhancement**:
```csharp
// In middleware or base controller
using (LogContext.PushProperty("UserId", currentUserId))
using (LogContext.PushProperty("Username", currentUsername))
{
    // All logs in this scope will include user context automatically
    _logger.LogInformation("Performing operation");
}
```

---

## 🔵 LOW PRIORITY TASKS

### 7. Documentation Updates
**Status**: NOT STARTED  
**Priority**: LOW  
**Effort**: Small

**Tasks**:
- [ ] Update `.github/copilot-instructions.md` with logging patterns
- [ ] Add logging examples to README.md
- [ ] Create TROUBLESHOOTING.md guide
- [ ] Document error handling patterns
- [ ] Update architecture diagrams if needed

### 8. Testing Infrastructure
**Status**: NOT STARTED  
**Priority**: LOW  
**Effort**: Large

**Tasks**:
- [ ] Set up xUnit test project
- [ ] Add unit tests for AuthenticationService
- [ ] Add unit tests for EntityService
- [ ] Add integration tests for controllers
- [ ] Add integration tests for repositories
- [ ] Configure test MongoDB connection
- [ ] Set up code coverage reporting

### 9. Logging Enhancements
**Status**: NOT STARTED  
**Priority**: LOW  
**Effort**: Small

**Tasks**:
- [ ] Add performance logging (execution time for slow operations)
- [ ] Add request/response logging for API endpoints (future)
- [ ] Implement log correlation IDs for request tracing
- [ ] Add custom log filters for sensitive data
- [ ] Create log aggregation and search dashboard (future)

---

## 📊 IMPLEMENTATION PRIORITY

### Week 1 (Current)
1. ✅ Complete logging system with user context
2. 🔴 Implement global error handling middleware
3. 🔴 Add validation improvements and UX enhancements

### Week 2
1. Complete EntityService logging for all entity types
2. Add MongoDB connection health checks
3. Begin Universe management UI implementation

### Week 3-4
1. Universe CRUD UI
2. NotableFigure CRUD UI
3. Basic relationship management

---

## 🎯 SUCCESS CRITERIA

### Error Handling
- [ ] No unhandled exceptions reach the user
- [ ] All errors logged with appropriate context
- [ ] User-friendly error messages displayed
- [ ] Consistent error response format

### Logging
- [x] All authentication operations logged
- [x] User context included in relevant logs
- [ ] All entity operations logged
- [x] Request/response logged automatically
- [x] Logs queryable in MongoDB
- [x] Logs rotated and retained properly

### UX
- [ ] Validation errors display inline
- [ ] Toast notifications for success/error
- [ ] Loading indicators on async operations
- [ ] Confirmation dialogs for destructive actions
- [ ] No sudden page errors without explanation

---

## 📝 NOTES

### Logging Best Practices Applied
- ✅ Structured logging with named properties
- ✅ User identification in audit logs
- ✅ Appropriate log levels (Debug, Information, Warning, Error)
- ✅ Exception logging with context
- ✅ No sensitive data (passwords, tokens) in logs

### Next Major Features (After Tasks)
1. Universe Management UI
2. Entity CRUD UI (all types)
3. Relationship Management UI
4. Search functionality
5. Visualization (graphs, timelines)

---

## 🔗 Related Documentation

- [TODO.md](TODO.md) - Complete project roadmap
- [README.md](README.md) - Project overview and setup
- [Glimmer.Core/Services/README_LOGGING.md](Glimmer.Core/Services/README_LOGGING.md) - Logging documentation
- [Glimmer.Core/Services/README_AUTHENTICATION.md](Glimmer.Core/Services/README_AUTHENTICATION.md) - Auth documentation
- [Glimmer.Core/Services/README_ENTITYSERVICE.md](Glimmer.Core/Services/README_ENTITYSERVICE.md) - Entity service documentation

---

**Last Updated**: 2025-11-04  
**Version**: 1.0
