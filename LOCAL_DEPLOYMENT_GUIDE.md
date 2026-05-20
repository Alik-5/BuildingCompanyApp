# 🏗️ LOCAL DEPLOYMENT GUIDE
## For Building Companies Installing BuildingPro Locally

**Last Updated:** May 20, 2026

---

## 📋 Table of Contents
1. [Overview: Why Local Deployment?](#overview-why-local-deployment)
2. [System Requirements](#system-requirements)
3. [Installation on a Single Computer](#installation-on-a-single-computer)
4. [Installation on Multiple Computers (Network Sharing)](#installation-on-multiple-computers-network-sharing)
5. [Database Storage & How It Works](#database-storage--how-it-works)
6. [Maintenance & Backups](#maintenance--backups)
7. [Troubleshooting](#troubleshooting)

---

## Overview: Why Local Deployment?

**Local deployment means:**
- The application runs on **your company's computer**, not in the cloud
- Your building company retains **full control** of your data
- **No recurring subscription costs** for cloud hosting
- **No internet required** to use the system (after initial setup)
- **Fast performance** - everything runs on your network
- **Data privacy** - your construction projects stay on your servers

### How It Works:

```
Building Company
├── Server PC (Windows/Linux with SQL Server)
│   ├── Web Application (runs on port 5000)
│   └── Database (stores all project data)
└── Employee Computers (on same network)
    ├── Open browser
    ├── Navigate to: http://server-ip:5000
    └── Access same application & database
```

---

## System Requirements

### For the Main Server (Where Database & App Run):

| Component | Requirement |
|-----------|-------------|
| **OS** | Windows 10/11, Windows Server 2019/2022, or Linux |
| **.NET Runtime** | .NET 8 (or .NET 8 SDK for development) |
| **Database** | SQL Server Express (free) or SQL Server 2019+ |
| **RAM** | Minimum 4GB, recommended 8GB+ |
| **Storage** | 10GB free (for OS, app, and database) |
| **Network** | LAN connection (for employee access) |

### For Employee Computers:
- Any web browser (Chrome, Edge, Firefox, Safari)
- Network connection to the main server
- No special software needed

---

## Installation on a Single Computer

### **Step 1: Install Prerequisites**

#### Install .NET 8 Runtime:
1. Go to: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
2. Download **"Runtime"** (or SDK if you plan to develop)
3. Install it (follow default settings)
4. Verify installation:
   ```powershell
   dotnet --version
   # Should show: 8.x.x
   ```

#### Install SQL Server Express:
1. Download from: https://www.microsoft.com/en-us/sql-server/sql-server-express
2. Choose **"Express"** (free version)
3. During installation:
   - Instance name: `SQLEXPRESS` (default)
   - Authentication: **Windows Authentication** (recommended)
   - Start service automatically: ✓ (checked)
4. Note the installation path for later backups

---

### **Step 2: Copy BuildingPro Application**

1. Copy the entire `BuildingCompanyApp` folder to your server:
   ```
   C:\BuildingPro\BuildingCompanyApp\
   ```
   (or any location you prefer)

2. Verify the folder contains:
   - `Controllers/`
   - `Models/`
   - `Views/`
   - `Services/`
   - `wwwroot/` (CSS, JS, images)
   - `Program.cs`
   - `appsettings.json`

---

### **Step 3: Configure Connection String**

Edit `appsettings.json` to match your setup:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local)\\SQLEXPRESS;Database=BuildingProDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

**Explanation:**
- `Server=(local)\SQLEXPRESS` - Your local SQL Server
- `Database=BuildingProDB` - Database name (can be any name)
- `Trusted_Connection=true` - Use Windows Authentication

---

### **Step 4: First Run Setup**

```powershell
# Navigate to the application folder
cd C:\BuildingPro\BuildingCompanyApp

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

**You should see:**
```
info: Microsoft.Hosting.Lifetime[14]
  Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
  Application started. Press Ctrl+C to exit.
```

---

### **Step 5: Access the Application**

1. Open a web browser
2. Go to: `http://localhost:5000`
3. Log in with default credentials:
   - **Username:** `admin`
   - **Password:** `admin123`

---

### **Step 6: Create Admin Users (Optional)**

After first login, go to **Admin Panel** → **Create User** to add your team members.

---

## Installation on Multiple Computers (Network Sharing)

### **Scenario:**
- Main server at your office (IP: 192.168.1.100)
- Employees access from their computers

### **Step 1-4: Set Up Main Server**
(Same as "Single Computer" above)

### **Step 5: Make Server Accessible on Network**

#### Modify `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=192.168.1.100\\SQLEXPRESS;Database=BuildingProDB;..."
  }
}
```

#### When starting the application:
```powershell
# Run the app and bind to all network interfaces
dotnet run --urls="http://*:5000"
```

**Your app is now accessible from:**
- `http://localhost:5000` (on server itself)
- `http://192.168.1.100:5000` (from other computers on network)

### **Step 6: Configure SQL Server for Network Access**

1. Open **SQL Server Configuration Manager**
2. Go to: **SQL Server Network Configuration** → **SQLEXPRESS** → **Protocols**
3. Enable **Named Pipes** and **TCP/IP**
4. Restart SQL Server service

### **Step 7: Employee Access**

On each employee's computer:
1. Open browser
2. Go to: `http://[server-ip]:5000`
3. Login with credentials

**Example:** If server IP is `192.168.1.100`:
```
http://192.168.1.100:5000
```

---

## Database Storage & How It Works

### **Where is Data Stored?**

```
SQL Server Installation
└── Data Folder (Default: C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\)
    ├── BuildingProDB.mdf (main database file)
    └── BuildingProDB_log.ldf (transaction log)
```

### **What Data Gets Stored?**

| Table | Contains | Example Data |
|-------|----------|--------------|
| **Users** | Login credentials, roles | Admin accounts, employee users |
| **Workers** | Employee information | Names, phone, employment status |
| **Projects** | Construction projects | Project name, location, budget |
| **ProjectWorkers** | Worker assignments | Who's working on which project |
| **Materials** | Building supplies | Cement, steel, paint inventory |
| **Finance** | Payments, salaries | Invoice amounts, worker payments |
| **Clients** | Client information | Company name, contact details |
| **Documents** | Project files | Blueprints, permits, contracts |
| **Tasks** | Worker tasks | "Paint wall 3", "Install window" |

### **How Data Flows:**

```
Employee Opens Browser → http://server:5000
                        ↓
                  Web Application
                        ↓
                  Business Logic (Services)
                        ↓
                  Entity Framework (ORM)
                        ↓
                  SQL Server Database
                        ↓
              Data stored in tables permanently
```

### **Example: Creating a Project**

1. **Manager** fills form: Project name = "Downtown Office Building"
2. **Browser** sends form data to server
3. **ProjectController** receives the request
4. **ProjectService** validates the data
5. **Repository** passes to Entity Framework
6. **Entity Framework** converts to SQL:
   ```sql
   INSERT INTO Projects (Name, Location, Budget, StartDate, ...)
   VALUES ('Downtown Office Building', '123 Main St', 500000, '2026-05-20', ...)
   ```
7. **SQL Server** executes and saves to disk
8. **Data is now permanent** - even if app restarts ✅

---

## Maintenance & Backups

### **Weekly Backup Script**

Create a PowerShell script `backup-database.ps1`:

```powershell
$BackupPath = "C:\BuildingProBackups"
$Date = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
$BackupFile = "$BackupPath\BuildingProDB_$Date.bak"

# Create backup folder if it doesn't exist
if (!(Test-Path $BackupPath)) {
    New-Item -ItemType Directory -Path $BackupPath | Out-Null
}

# Backup SQL Server database
sqlcmd -S (local)\SQLEXPRESS -Q "BACKUP DATABASE [BuildingProDB] TO DISK = '$BackupFile'"

Write-Host "Backup completed: $BackupFile"
```

**Run it weekly:**
1. Open Task Scheduler
2. Create task: Run `backup-database.ps1` every Friday at 5 PM

---

### **Automatic Restart (Optional)**

Create a batch file `start-buildingpro.bat`:

```batch
@echo off
cd C:\BuildingPro\BuildingCompanyApp
dotnet run --urls="http://*:5000"
```

**Add to Windows Startup:**
1. Press `Win+R` → `shell:startup`
2. Create shortcut to `start-buildingpro.bat`
3. App will auto-start when server reboots

---

## Troubleshooting

### **"Port 5000 already in use"**

```powershell
# Find what's using port 5000
Get-NetTCPConnection -LocalPort 5000

# Stop the process (replace PID with the number shown)
Stop-Process -Id [PID] -Force

# Or use different port
dotnet run --urls="http://*:5001"
```

---

### **"Cannot connect to SQL Server"**

Check SQL Server is running:
```powershell
# Check SQL Server service status
Get-Service -Name "MSSQL$SQLEXPRESS"

# Start if stopped
Start-Service -Name "MSSQL$SQLEXPRESS"
```

---

### **"Database not found"**

The database will auto-create on first run. If it doesn't:

```powershell
# Run Entity Framework migrations
dotnet ef database update
```

---

### **Employees Can't Access from Their Computers**

1. **Verify network:**
   ```powershell
   ping 192.168.1.100  # Replace with server IP
   ```

2. **Verify SQL Server accepts network connections:**
   ```powershell
   # On server:
   netstat -an | findstr 5000
   ```

3. **Check Windows Firewall:**
   - Allow port 5000 through firewall
   - Or disable firewall for internal network

---

## Summary

✅ **Local Deployment Advantages:**
- No cloud subscription costs
- Full data control and privacy
- Works offline (after initial setup)
- Fast performance on local network
- Easy backups and maintenance

✅ **What Happens When:**
- **App starts:** Connects to local SQL Server → Ready
- **User logs in:** Queries Users table → Validates credentials
- **Manager creates project:** Inserts into Projects table → Data saved permanently
- **Employee views tasks:** Queries Tasks table → Shows assigned work
- **Admin backs up:** Exports database file → Stored safely

---

**Questions?** Check [README.md](README.md) or documentation files.

