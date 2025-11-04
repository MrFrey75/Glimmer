# Glimmer - Quick Reference Guide

**Last Updated**: 2025-11-04

---

## 🚀 Quick Start

```bash
# Start MongoDB (if using Docker)
docker start mongodb

# Run the application
cd Glimmer.Creator
dotnet run

# Navigate to
https://localhost:5228

# Login
Username: Admin
Password: Password1234
```

---

## 📁 Key Files

| File | Purpose |
|------|---------|
| `TASKS.md` | **Immediate priorities and next steps** ⭐ |
| `TODO.md` | Complete project roadmap |
| `IMPLEMENTATION_SUMMARY.md` | Recent work summary |
| `README.md` | Project overview and setup |
| `Glimmer.Core/Services/README_LOGGING.md` | Logging documentation |

---

## 🎯 Current Priorities (See TASKS.md)

### 🔴 HIGH PRIORITY
1. **Error Handling** - Global exception middleware needed
2. **Validation UX** - Toast notifications and client-side validation
3. **Universe Management UI** - CRUD interface for universes

### 🟡 MEDIUM PRIORITY
1. **Complete EntityService Logging** - Add to remaining entity types
2. **MongoDB Health Checks** - Connection validation on startup
3. **User Context Middleware** - Automatic user context in logs

---

## 📊 Project Status

- **Core Infrastructure**: 100% ✅
- **Authentication**: 100% ✅
- **Logging System**: 100% ✅ (just completed)
- **Web UI**: 20% 🚧
- **Testing**: 0% ⏳

**Overall**: ~40% complete

---

## 🛠️ Development Commands

```bash
# Build solution
dotnet build

# Run with hot reload
cd Glimmer.Creator
dotnet watch run

# View logs
tail -f Glimmer.Creator/Logs/glimmer-$(date +%Y%m%d).log

# MongoDB logs query
mongosh GlimmerDB --eval "db.Logs.find().sort({Timestamp:-1}).limit(10)"

# Check MongoDB connection
mongosh --eval "db.version()"
```

---

## 🔍 Log Queries

### Find user activities
```javascript
db.Logs.find({
  "Properties.Username": "Admin"
}).sort({ Timestamp: -1 }).limit(20)
```

### Find errors in last hour
```javascript
db.Logs.find({
  Level: "Error",
  Timestamp: { $gte: new Date(Date.now() - 3600000) }
}).sort({ Timestamp: -1 })
```

### Find failed logins
```javascript
db.Logs.find({
  Level: "Warning",
  Message: /Login failed/
}).sort({ Timestamp: -1 })
```

---

## 📖 Architecture

```
Web Browser (Dark Mode)
        ↓
Glimmer.Creator (MVC)
   Controllers → Services
        ↓
Glimmer.Core (Business Logic)
   Services → Repositories
        ↓
MongoDB (Persistence)
   Collections: users, universes, relations, logs
```

---

## 🔐 Security Notes

- Default superuser: Admin/Password1234 (change immediately!)
- JWT tokens: 60 min (access), 7 days (refresh)
- Passwords: HMACSHA512 with unique salts
- Superuser cannot be deleted or deactivated
- All auth operations logged with user context

---

## 🐛 Troubleshooting

### MongoDB not running
```bash
# Docker
docker start mongodb

# Local (Ubuntu)
sudo systemctl start mongod
```

### Port already in use
```bash
# Find process using port 5228
lsof -i :5228

# Kill process
kill -9 <PID>
```

### Build errors
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

---

## 📞 Need Help?

1. Check **TASKS.md** for current priorities
2. Check **TODO.md** for full roadmap
3. Check **IMPLEMENTATION_SUMMARY.md** for recent changes
4. Check service-specific READMEs in `Glimmer.Core/Services/`
5. Review error logs in `Glimmer.Creator/Logs/`
6. Query MongoDB logs collection

---

## 🎓 Next Steps for New Developers

1. Read `README.md` for project overview
2. Review `TASKS.md` for immediate priorities
3. Check `.github/copilot-instructions.md` for patterns
4. Start with highest priority task in TASKS.md
5. Follow existing logging patterns in services

---

**Pro Tip**: The codebase uses structured logging. Always include user context (Username, UserId) in logs where applicable.

**Pro Tip 2**: MongoDB is the source of truth. All operations go through repositories. Never bypass the repository pattern.

**Pro Tip 3**: Dark mode is always on. UI colors: #1a1a1a (bg), #e0e0e0 (text), #9333ea (accent).
