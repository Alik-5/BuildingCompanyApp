# 🚀 Qore - Quick Start Deployment Guide

## 📱 What Is Qore?

**Qore** is a professional construction management platform that helps you:
- ✅ Manage projects, employees, materials, finance, and clients
- ✅ Assign tasks and track progress
- ✅ Store and organize documents
- ✅ Access from anywhere with role-based permissions

---

## 🎯 Quick Summary: Three Ways to Deploy

| Method | Time | Cost | Data Saved | Access |
|--------|------|------|-----------|---------|
| **Local Computer** | 5 min | Free | ❌ Restart = Lost | `localhost:5000` |
| **Another PC** | 15 min | Free | ❌ Restart = Lost | Local Network |
| **Cloud (Render)** | 30 min | Free-$7/mo | ✅ Permanent | Worldwide URL |

---

## 1️⃣ Run Qore on Your Computer (Fastest)

### Prerequisites
- Download **[.NET 8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)** (≈120MB)
- Install it
- That's it!

### Run
```bash
cd C:\Users\YourName\Desktop\Qore\BuildingCompanyApp
dotnet run
```

### Access
Open browser: **http://localhost:5000**

### Demo Credentials
```
Username: admin
Password: admin123
```

### Data
- ⚠️ **WARNING**: When you close the app, all new data disappears!
- Use this for: Testing, development, personal use

---

## 2️⃣ Run Qore on Another Computer (Portable)

### Step 1: On Source Computer
Copy entire folder:
```
Qore/BuildingCompanyApp/  →  (USB Drive or Cloud)
```

### Step 2: On Target Computer
1. Install .NET 8 Runtime
2. Extract folder to: `C:\Apps\Qore\`
3. Open Terminal/PowerShell in that folder
4. Run:
```bash
dotnet restore
dotnet run
```

### Access from Network
Other PCs can access via:
```
http://[YOUR-PC-IP]:5000
```

Find your IP:
```bash
ipconfig
```
Look for: `IPv4 Address: 192.168.x.x`

### Data
- ⚠️ Still **in-memory** (lost on restart)
- For: Team testing, local network deployment

---

## 3️⃣ Deploy to Cloud (Render) - RECOMMENDED FOR PRODUCTION ☁️

### Prerequisites
- GitHub account (free at github.com)
- Render account (free at render.com)

### Step 1: Create GitHub Repository

```bash
cd C:\Users\YourName\Desktop\Qore\BuildingCompanyApp

# Initialize git
git init
git add .
git commit -m "Initial Qore deployment"
git branch -M main

# Create repo on github.com, then:
git remote add origin https://github.com/YOUR_USERNAME/qore.git
git push -u origin main
```

### Step 2: Deploy to Render

1. Go to **https://render.com**
2. Sign up with GitHub
3. Click **"New +"** → **"Web Service"**
4. Select your `qore` repository
5. Settings:
   - **Name:** `qore`
   - **Environment:** `.NET`
   - **Build Command:** `dotnet build`
   - **Start Command:** `dotnet run --urls "http://0.0.0.0:$PORT"`
   - **Environment Variables:**
     ```
     ASPNETCORE_ENVIRONMENT=Production
     ```

6. Click **"Create Web Service"**
7. Wait 2-3 minutes for deployment
8. Your app is live at: `https://qore-XXXX.onrender.com`

### Data Storage (Cloud)
✅ **Data persists** using in-memory storage during app uptime
⚠️ Render free tier: App sleeps after 15 min inactivity (resets data)
💾 **For permanent storage**: Upgrade to paid tier or add SQL Server

### Access
```
https://qore-XXXX.onrender.com (worldwide access)
Username: admin
Password: admin123
```

---

## 🔐 Creating New Manager/Admin Accounts

### Method 1: Through Admin Panel (Recommended for Production)
1. Log in as `admin`
2. Click **🛡️ Admin Panel** (in sidebar, only visible for admins)
3. Click **➕ Create New User**
4. Fill form and select Role (Admin/Manager/Employee)
5. New user can log in immediately

### Method 2: Direct Code Edit (Development Only)
Edit `Services/AppDataStore.cs`:

```csharp
public static List<User> Users { get; } =
[
    CreateUser(1, "admin", "admin@qore.am", "Qore", "Admin", "Admin", "admin123"),
    
    // ADD NEW MANAGER:
    CreateUser(7, "david.manager", "david@qore.am", "David", "Hakobyan", "Manager", "david789"),
    
    // ADD NEW ADMIN:
    CreateUser(8, "admin2", "admin2@qore.am", "Qore", "Admin2", "Admin", "admin456"),
];
```

Then rebuild: `dotnet build && dotnet run`

---

## 📊 User Roles Explained

### 👤 **Employee**
- See: Only their own tasks, assigned projects, and salary
- Can: Update task progress, view documents
- Cannot: Create projects, manage other employees

**Example:** Construction worker John sees only his tasks

### 👨‍💼 **Manager**
- See: All projects, all employees, all materials, all finance
- Can: Create tasks, assign employees, manage materials
- Cannot: Delete users, system settings

**Example:** Foreman Karen manages the whole site

### 🛡️ **Admin**
- See: **Everything** in the system
- Can: Create/delete users, manage all settings, full control
- Cannot: More permissions = more responsibility!

**Example:** Company owner gets full system access

---

## 💾 Understanding Data Storage

### Current Setup: In-Memory (AppDataStore.cs)
```
App Starts → Loads demo data → You work with it
   ↓
App Restarts → Data lost (only demo data back)
```

**Data is stored in:** RAM (computer memory)
**Survives restart?** No ❌
**Good for?** Testing, demo, development

### Production Setup: SQL Server (Future)
```
App Starts → Connects to SQL Server
   ↓
You create data → Saved to database
   ↓
App Restarts → Data still there! ✅
```

**Data is stored in:** SQL Server database (disk)
**Survives restart?** Yes ✅
**Good for?** Business use, production

---

## 📋 Admin Panel Features

Once deployed, Admin panel (at `/Admin/Dashboard`) allows:

✅ View all users  
✅ Create new Manager/Admin accounts  
✅ Edit user details  
✅ Deactivate users  
✅ View user statistics  

**Access:** Log in as Admin → Click 🛡️ Admin Panel (sidebar)

---

## ⚡ System Requirements

### Minimum (Just to Run)
- **OS:** Windows 10+ OR Linux OR Mac
- **.NET 8 Runtime:** 120MB disk space
- **RAM:** 512MB
- **Internet:** Not required for local

### Recommended (For Production Cloud)
- **GitHub Account** (free)
- **Render Account** (free)
- **SQL Server** (when data grows)
- **Backup strategy** (critical!)

---

## 🌐 Deployment Architecture Diagram

### Local Computer
```
Your PC
  ├── Qore Application
  ├── .NET Runtime
  └── In-Memory Data (RAM)
         ↓
    http://localhost:5000
```

### Company Network
```
Office PC
  ├── Qore Application
  ├── .NET Runtime
  └── In-Memory Data (RAM)
         ↓
Other PCs: http://192.168.x.x:5000
```

### Cloud (Recommended)
```
Render Server (Internet)
  ├── Qore Application
  ├── .NET Runtime
  ├── SQL Server Database ✅
  └── Persistent Storage ✅
         ↓
Worldwide: https://qore-xxxx.onrender.com
```

---

## ✅ Deployment Checklist

### Before Going Live

- [ ] Change all demo passwords
- [ ] Create real user accounts (not demo)
- [ ] Test login with different roles
- [ ] Verify all features work
- [ ] Plan data backup strategy
- [ ] Enable HTTPS (cloud platforms do this)
- [ ] Test on different devices/browsers
- [ ] Create admin documentation for your team

---

## 🎓 Next Steps

### Learn More
1. **AUTH_AND_ACCOUNTS.md** - Complete authentication guide
2. **INSTALLATION_AND_DEPLOYMENT.md** - Detailed deployment steps
3. **README.md** - Feature overview

### Upgrade to SQL Server
When you're ready to make data permanent:
1. Install SQL Server
2. Update connection string
3. Run migrations
4. Deploy again

### Add More Features
- Email notifications
- Two-factor authentication
- Advanced reporting
- Mobile app
- API for integrations

---

## 🆘 Troubleshooting

### Q: App won't start
**A:** Make sure .NET 8 is installed
```bash
dotnet --version
```

### Q: Can't access from another PC
**A:** Check your IP address and firewall
```bash
ipconfig
```

### Q: Lost all data after restart
**A:** You're using in-memory storage. This is expected!
**Solution:** Migrate to SQL Server for permanent storage

### Q: Password not working
**A:** Use demo accounts:
- `admin` / `admin123`
- `karen.manager` / `karen123`

### Q: Port 5000 already in use
**A:** Run on different port:
```bash
dotnet run --urls "http://localhost:5001"
```

---

## 📞 Support

For detailed information, check these files in the Qore folder:
- `README.md` - Features overview
- `AUTH_AND_ACCOUNTS.md` - User management
- `INSTALLATION_AND_DEPLOYMENT.md` - Full deployment guide
- `QUICK_START.md` - Getting started

---

## 🎉 You're Ready!

Choose one:
1. **Just test?** → Run locally (Method 1)
2. **Share with team?** → Deploy on network (Method 2)
3. **Go production?** → Deploy to Render (Method 3)

**Start now:**
```bash
cd BuildingCompanyApp
dotnet run
```

**Open browser:** `http://localhost:5000`

**Welcome to Qore!** 🚀

