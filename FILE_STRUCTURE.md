# Project File Structure & Navigation Guide

## Complete Directory Structure

```
BuildingCompanyApp/
│
├── 📁 Data/
│   ├── database-schema.sql              # Complete MSSQL database schema
│   └── ApplicationDbContext.cs          # Entity Framework context (optional)
│
├── 📁 Models/
│   ├── User.cs                          # User & authentication models
│   ├── Worker.cs                        # Worker model
│   ├── Project.cs                       # Project & related models
│   └── Document.cs                      # Document model
│
├── 📁 Controllers/
│   ├── AuthController.cs                # Login, register, logout
│   ├── DashboardController.cs           # Dashboard metrics
│   ├── ProjectController.cs             # Workers & projects management
│   └── DocumentController.cs            # Document upload & management
│
├── 📁 Services/
│   ├── AuthService.cs                   # Authentication logic
│   ├── WorkerService.cs                 # Worker business logic
│   ├── ProjectService.cs                # Project business logic
│   ├── DocumentService.cs               # Document handling
│   ├── DashboardService.cs              # Dashboard metrics
│   └── IRepository.cs                   # Repository interfaces
│
├── 📁 Views/
│   ├── 📁 Auth/
│   │   ├── Login.cshtml                 # Login page
│   │   └── Register.cshtml              # User registration
│   │
│   ├── 📁 Dashboard/
│   │   └── Index.cshtml                 # Dashboard with metrics
│   │
│   ├── 📁 Worker/
│   │   ├── Index.cshtml                 # Workers list view
│   │   ├── Create.cshtml                # Add new worker form
│   │   ├── Edit.cshtml                  # Edit worker form
│   │   └── Details.cshtml               # Worker profile
│   │
│   ├── 📁 Project/
│   │   ├── Index.cshtml                 # Projects list
│   │   └── Create.cshtml                # Create project form
│   │
│   ├── 📁 Document/
│   │   ├── Index.cshtml                 # Documents list
│   │   └── Upload.cshtml                # Document upload form
│   │
│   └── 📁 Shared/
│       └── _Layout.cshtml               # Main layout template
│
├── 📁 wwwroot/
│   ├── 📁 css/
│   │   ├── style.css                    # Main application styles
│   │   └── responsive.css               # Mobile responsive design
│   │
│   ├── 📁 js/
│   │   └── main.js                      # JavaScript functionality
│   │
│   ├── 📁 images/                       # Static images
│   │   └── (add your logos here)
│   │
│   └── 📁 uploads/
│       ├── 📁 documents/                # Uploaded documents
│       └── 📁 project-images/           # Project photos
│
├── 📄 Program.cs                        # Application startup & config
├── 📄 appsettings.json                  # App configuration
├── 📄 BuildingCompanyApp.csproj         # Project file
├── 📄 package.json                      # Package information
│
├── 📄 README.md                         # Main documentation
├── 📄 QUICK_START.md                    # Quick start guide
├── 📄 ARCHITECTURE.md                   # System architecture
├── 📄 DEPLOYMENT_COMPLETE.md            # Complete deployment guide
├── 📄 DEPLOYMENT_CHECKLIST.md           # Pre-launch checklist
├── 📄 .gitignore                        # Git ignore file
└── 📄 LICENSE                           # MIT License (optional)
```

## Documentation Files Explanation

### 1. **README.md** (Start Here!)
- Project overview
- Features list
- Technology stack
- Local setup instructions
- Basic deployment introduction

### 2. **QUICK_START.md**
- Fast setup for development
- Common commands
- Troubleshooting tips
- Quick deployment options

### 3. **ARCHITECTURE.md**
- System design patterns
- Application flow diagrams
- Database relationships
- Development workflow
- Testing approaches

### 4. **DEPLOYMENT_COMPLETE.md** (Comprehensive!)
- Step-by-step deployment for all platforms
- Azure, AWS, DigitalOcean, self-hosted
- Domain and SSL setup
- Post-deployment configuration
- Troubleshooting guide

### 5. **DEPLOYMENT_CHECKLIST.md**
- Pre-launch security checklist
- Database preparation
- Application configuration
- Infrastructure setup
- Testing requirements

## Key Files By Purpose

### For Development
- `Program.cs` - Entry point and configuration
- `appsettings.json` - Connection strings and settings
- `BuildingCompanyApp.csproj` - Dependencies and project settings

### For Frontend
- `wwwroot/css/style.css` - Main styling
- `wwwroot/css/responsive.css` - Mobile responsive
- `wwwroot/js/main.js` - Client-side JavaScript

### For Database
- `Data/database-schema.sql` - Create database structure
- `Data/ApplicationDbContext.cs` - Entity Framework setup

### For Business Logic
- `Services/` - All business logic implementations
- `Controllers/` - HTTP request handlers

### For User Interface
- `Views/` - All HTML templates
- `Models/` - Data structures

## How to Navigate the Code

### Adding a New Feature
1. Create Model in `Models/`
2. Create Service in `Services/`
3. Create Controller in `Controllers/`
4. Create View in `Views/`
5. Add routes if needed in `Program.cs`

### Making Database Changes
1. Modify model in `Models/`
2. Update `Data/database-schema.sql`
3. Run SQL script to update database
4. Test changes

### Styling Changes
1. Edit `wwwroot/css/style.css` for desktop
2. Edit `wwwroot/css/responsive.css` for mobile
3. Test on different devices

### JavaScript Changes
1. Edit `wwwroot/js/main.js`
2. Add event listeners or utility functions
3. Test in browser console

## Important Notes

### Connection String Location
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=BuildingCompanyDB;..."
}
```

### Authentication
- Uses BCrypt password hashing
- Session-based (30 min timeout)
- Located in `Services/AuthService.cs`

### Database Setup
- SQL script: `Data/database-schema.sql`
- Run once during setup
- Creates all tables and indexes

### Responsive Design
- Desktop-first CSS in `style.css`
- Mobile overrides in `responsive.css`
- Breakpoints: 768px (tablet), 480px (mobile)

## Running the Application

### Development
```bash
dotnet run
# Access: http://localhost:5000
```

### Production Build
```bash
dotnet publish -c Release -o ./publish
# Deploy ./publish folder to server
```

## Testing Locally

### Database Connection
```bash
# Test via SSMS or:
sqlcmd -S localhost -Q "SELECT @@VERSION"
```

### Application
- Navigate to all pages
- Test CRUD operations
- Test responsiveness (F12 in browser)
- Check console for errors

## Deployment Quick Reference

| Platform | Setup Time | Cost | Difficulty |
|----------|-----------|------|-----------|
| Azure | 30 min | Free tier available | Easy |
| AWS | 45 min | Free tier available | Medium |
| DigitalOcean | 20 min | $5-6/month | Easy |
| Self-Hosted | 60 min | Domain + VPS cost | Hard |

## File Size References

- `style.css` - ~15 KB
- `responsive.css` - ~8 KB
- `main.js` - ~5 KB
- Database schema - ~3 KB

## Security Files

- `.gitignore` - Prevent sensitive files in git
- Connection strings - Keep in `appsettings.json` (don't commit)
- Passwords - Use environment variables in production

## Getting Started Checklist

- [ ] Read README.md
- [ ] Run local setup (QUICK_START.md)
- [ ] Create database (database-schema.sql)
- [ ] Run application locally
- [ ] Test all features
- [ ] Choose deployment platform
- [ ] Follow DEPLOYMENT_COMPLETE.md
- [ ] Complete DEPLOYMENT_CHECKLIST.md
- [ ] Deploy!

---

**Need Help?**
- Start with README.md
- Check QUICK_START.md for common issues
- Review ARCHITECTURE.md for code structure
- Follow DEPLOYMENT_COMPLETE.md step by step
