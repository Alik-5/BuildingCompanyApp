# Qore - Complete Installation & Deployment Guide

## 📋 Table of Contents
1. [Local Development Setup](#local-development-setup)
2. [Installation on Another Computer](#installation-on-another-computer)
3. [Installation on a Server (Render)](#installation-on-a-server-render)
4. [Data Storage Explained](#data-storage-explained)
5. [Production Checklist](#production-checklist)
6. [Troubleshooting](#troubleshooting)

---

## Local Development Setup

### Prerequisites
- **Windows 10/11** (or Linux/macOS with .NET support)
- **.NET 8 SDK** (download from https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **Git** (optional, but recommended)

### Step 1: Clone or Extract Qore
```bash
# Extract the Qore project to your folder
cd C:\Users\YourUsername\Desktop\Qore
```

### Step 2: Restore Dependencies
```bash
cd BuildingCompanyApp
dotnet restore
```

### Step 3: Build the Project
```bash
dotnet build
```

### Step 4: Run Locally
```bash
dotnet run
```

The app will start at: **http://localhost:5000**

### Step 5: Log In
- **Demo Admin Account:**
  - Username: `admin`
  - Password: `admin123`

---

## Installation on Another Computer

### What You Need to Transfer
```
Qore/
├── BuildingCompanyApp/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Views/
│   ├── wwwroot/          (CSS, JS, images)
│   ├── appsettings.json
│   ├── Program.cs
│   ├── BuildingCompanyApp.csproj
│   └── ... (all source files)
└── README.md
```

### Installation Steps on Another Computer

#### Step 1: Install .NET 8 Runtime (OR SDK)
**Option A: Minimal Installation (Runtime Only)**
- Download: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- Choose **Runtime** (smaller, ~120MB)
- Install it

**Option B: Full Installation (Development)**
- Download: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- Choose **SDK** (larger, ~700MB)
- Install it

#### Step 2: Copy Qore Folder
Copy the entire `BuildingCompanyApp` folder to the target computer:
```
C:\Apps\Qore\BuildingCompanyApp\
```

#### Step 3: Restore & Run
```bash
cd C:\Apps\Qore\BuildingCompanyApp
dotnet restore
dotnet run
```

#### Step 4: Access Qore
Open browser: **http://localhost:5000**

---

## Installation on a Server (Render)

Render is a free/paid cloud platform for deploying .NET apps.

### Step 1: Prepare Your Git Repository

```bash
# Initialize git (if not already done)
cd C:\Users\aaleq\Desktop\Qore\BuildingCompanyApp
git init
git add .
git commit -m "Initial Qore commit"
git branch -M main

# Add GitHub remote (create free account at github.com if needed)
git remote add origin https://github.com/YOUR_USERNAME/qore.git
git push -u origin main
```

### Step 2: Create Render Account
1. Go to **https://render.com**
2. Sign up with GitHub account
3. Click "New +" → "Web Service"
4. Connect your GitHub repository

### Step 3: Configure Render Settings

**Name:** `qore-app`
**Environment:** `.NET`
**Build Command:**
```
dotnet build
```

**Start Command:**
```
dotnet run --urls "http://0.0.0.0:$PORT"
```

**Environment Variables:**
```
ASPNETCORE_ENVIRONMENT=Production
DOTNET_TieredCompilation=false
```

### Step 4: Advanced Configuration (Optional)

**For SQL Server (Production Data):**
Add to `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=qore;User Id=sa;Password=YOUR_PASSWORD;"
  }
}
```

Then add to Render Environment Variables:
```
ConnectionStrings__DefaultConnection=Server=your-server;Database=qore;...
```

### Step 5: Deploy
- Click **"Create Web Service"**
- Render will automatically build and deploy
- Your app will be live at: `https://qore-app.onrender.com`

---

## Data Storage Explained

### Current Architecture: In-Memory Storage

```
┌─────────────────────────────────────┐
│   Qore Application (ASP.NET Core)   │
├─────────────────────────────────────┤
│                                     │
│   AppDataStore.cs (RAM)             │
│   ├── Users[]                       │
│   ├── Workers[]                     │
│   ├── Projects[]                    │
│   ├── Tasks[]                       │
│   ├── Materials[]                   │
│   ├── Finance[]                     │
│   ├── Clients[]                     │
│   └── Documents[]                   │
│                                     │
└─────────────────────────────────────┘
         (Memory Only)
```

### Where Is Data Saved NOW?
- **Location:** `Services/AppDataStore.cs`
- **Storage:** Application RAM (volatile)
- **Persistence:** **ONLY while app is running**
- **Data Loss:** When app restarts/crashes, all new data is lost!

### How Data Flows
```
1. User fills form (e.g., "Create Task")
   ↓
2. Controller receives request
   ↓
3. Service processes business logic
   ↓
4. Repository adds to AppDataStore list in RAM
   ↓
5. Page refreshes with new data
   ↓
6. APP RESTARTS → DATA LOST ❌
```

### Production: SQL Server Storage

For persistent storage, replace in-memory with SQL Server:

```
┌─────────────────────────────────────┐
│   Qore Application (ASP.NET Core)   │
├─────────────────────────────────────┤
│   Entity Framework Core (ORM)       │
│   (Automatically maps to database)  │
└──────────────┬──────────────────────┘
               │
        ┌──────▼──────────┐
        │  SQL Server DB  │
        ├─────────────────┤
        │ Users table     │
        │ Workers table   │
        │ Projects table  │
        │ Tasks table     │
        │ Materials table │
        │ Finance table   │
        │ Clients table   │
        │ Documents table │
        │ Invoices table  │
        └─────────────────┘
        (Persistent Storage)
```

### How to Enable SQL Server Persistence

#### Option 1: Local SQL Server (Development)

```bash
# Install SQL Server Express (free)
# Download: https://www.microsoft.com/en-us/sql-server/sql-server-express

# In appsettings.json, add:
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=qore;Integrated Security=true;"
}

# Run migrations:
dotnet ef database update
```

#### Option 2: Azure SQL Database (Cloud)

```bash
# Create Azure account: https://azure.microsoft.com

# Create SQL Database in Azure Portal

# Update appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=qore-server.database.windows.net;Database=qore;User Id=admin@qore;Password=YourStrongPassword;"
}

# Deploy migrations
dotnet ef database update
```

---

## Production Checklist

### Before Going Live

- [ ] **Change all demo passwords**
  ```
  Admin password (current: admin123)
  Manager password (current: karen123)
  All employee passwords
  ```

- [ ] **Enable HTTPS**
  ```csharp
  // In Program.cs
  app.UseHttpsRedirection();
  ```

- [ ] **Set environment to Production**
  ```
  ASPNETCORE_ENVIRONMENT=Production
  ```

- [ ] **Set up proper database**
  - [ ] SQL Server or Azure SQL
  - [ ] Run migrations
  - [ ] Backup strategy

- [ ] **Configure error logging**
  ```csharp
  // Install Serilog or Application Insights
  ```

- [ ] **Set up backups**
  - [ ] Database backups (daily)
  - [ ] Document storage backups

- [ ] **Enable authentication**
  - [ ] Force strong passwords
  - [ ] Password reset functionality
  - [ ] Two-factor authentication (optional)

- [ ] **Performance tuning**
  - [ ] Enable response compression
  - [ ] Add caching headers
  - [ ] Database indexing

- [ ] **Security headers**
  ```csharp
  // Prevent clickjacking
  app.Use(async (context, next) =>
  {
      context.Response.Headers.Add("X-Frame-Options", "DENY");
      context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
      await next();
  });
  ```

---

## Deployment Architecture Comparison

### Local Computer
```
┌──────────────────────┐
│  Your Computer       │
│  ├── .NET Runtime    │
│  ├── Qore App        │
│  └── RAM (in-memory) │
└──────────────────────┘
Access: http://localhost:5000
```

### Another Company Computer
```
┌──────────────────────────┐
│  Office Computer         │
│  ├── .NET Runtime        │
│  ├── Qore App            │
│  └── RAM (in-memory)     │
└──────────────────────────┘
Access: http://192.168.1.50:5000
(via local network)
```

### Cloud Server (Render)
```
┌────────────────────────────────┐
│  Render Server (Cloud)         │
│  ├── .NET Runtime              │
│  ├── Qore App                  │
│  └── SQL Server Database       │
└────────────────────────────────┘
Access: https://qore-app.onrender.com
(from anywhere in the world)
```

---

## Troubleshooting

### App won't start
**Error:** `"The process cannot access the file because it is being used by another process"`

**Solution:**
```bash
# Stop any running instances
taskkill /F /IM BuildingCompanyApp.exe

# Then try running again
dotnet run
```

### Port 5000 already in use
**Error:** `Address already in use`

**Solution:**
```bash
# Run on different port
dotnet run --urls "http://localhost:5001"
```

### Database connection failed
**Error:** `"Cannot connect to server..."`

**Solution:**
```bash
# Check connection string in appsettings.json
# Verify SQL Server is running
# Check firewall rules
```

### Login not working
**Error:** `"Invalid username or password"`

**Verify:**
```
1. Check demo accounts exist in AppDataStore.cs
2. Ensure passwords are correctly hashed
3. Check Session is properly configured
```

### Files not found after restart
**Issue:** New tasks/projects disappeared

**Cause:** Using in-memory storage (AppDataStore)

**Solution:** 
- Migrate to SQL Server database
- Or don't restart the app

---

## Next Steps: Migrate to Persistent Storage

To move from in-memory to real database storage:

### 1. Install Entity Framework Core
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### 2. Configure Database in Program.cs
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

### 3. Create migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Update Repository classes
Replace in-memory lists with EF Core queries.

---

## Summary

| Aspect | Local | Another Computer | Cloud (Render) |
|--------|-------|------------------|---|
| Setup Time | 5 min | 15 min | 30 min |
| Data Persistence | ❌ In-memory | ❌ In-memory | ✅ SQL Server |
| Cost | Free | Free | Free/Paid ($7+/mo) |
| Accessibility | localhost only | Local network | Worldwide |
| Backup | Manual | Manual | Automatic |
| SSL Certificate | None | Self-signed | Free (Let's Encrypt) |

---

**Questions?** Check `README.md` or `AUTH_AND_ACCOUNTS.md` for more info.

**Ready to go production?** Switch to SQL Server and follow the Production Checklist!

