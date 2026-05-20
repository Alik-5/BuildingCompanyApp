# Building Company Management System

A comprehensive web application for managing building/construction companies with workers, projects, documents, and authentication.

## Features

- **Authentication**: Secure login and registration system with password hashing
- **Dashboard**: Real-time metrics and overview of company operations
- **Workers Management**: Create, read, update, delete worker profiles with specializations
- **Projects Management**: Track projects with status, deadlines, budgets, and images
- **Documents Management**: Upload, store, and manage company documents
- **Responsive Design**: Fully responsive UI that works on desktop, tablet, and mobile devices
- **Role-Based Access**: Different user roles (Admin, Manager, Worker)

## Project Structure

```
BuildingCompanyApp/
├── Data/
│   └── database-schema.sql         # MSSQL database schema
├── Models/
│   ├── User.cs                     # User and authentication models
│   ├── Worker.cs                   # Worker model
│   ├── Project.cs                  # Project and related models
│   └── Document.cs                 # Document model
├── Controllers/
│   ├── AuthController.cs           # Authentication controller
│   ├── DashboardController.cs      # Dashboard controller
│   ├── ProjectController.cs        # Project and Worker controller
│   └── DocumentController.cs       # Document controller
├── Services/
│   ├── AuthService.cs              # Authentication logic
│   ├── WorkerService.cs            # Worker business logic
│   ├── ProjectService.cs           # Project business logic
│   ├── DocumentService.cs          # Document business logic
│   ├── DashboardService.cs         # Dashboard metrics
│   └── IRepository.cs              # Repository interfaces
├── Views/
│   ├── Auth/
│   │   ├── Login.cshtml            # Login page
│   │   └── Register.cshtml         # Registration page
│   ├── Dashboard/
│   │   └── Index.cshtml            # Dashboard page
│   ├── Worker/
│   │   ├── Index.cshtml            # Workers list
│   │   └── Create.cshtml           # Add worker form
│   ├── Project/
│   │   └── Index.cshtml            # Projects list
│   ├── Document/
│   │   └── Index.cshtml            # Documents list
│   └── Shared/
│       └── _Layout.cshtml          # Main layout
├── wwwroot/
│   ├── css/
│   │   ├── style.css               # Main styles
│   │   └── responsive.css          # Responsive design
│   ├── js/
│   │   └── main.js                 # JavaScript functionality
│   ├── images/                     # Static images
│   └── uploads/                    # User uploads
├── Program.cs                      # Application startup
├── appsettings.json                # Configuration
├── BuildingCompanyApp.csproj       # Project file
└── README.md                       # This file
```

## Technology Stack

- **Frontend**: HTML5, CSS3, JavaScript (Vanilla)
- **Backend**: C# with ASP.NET Core 7.0
- **Database**: Microsoft SQL Server (MSSQL)
- **Authentication**: BCrypt password hashing
- **Session Management**: ASP.NET Core Session

## Prerequisites

Before running this application, ensure you have:

1. **.NET 7.0 SDK** - Download from [microsoft.com/net](https://dotnet.microsoft.com/download)
2. **SQL Server** - Install SQL Server Express or SQL Server Developer Edition
3. **Visual Studio 2022** or **Visual Studio Code** with C# extension
4. **Git** (optional, for version control)

### Installation Steps

1. **Download and Install .NET 7.0 SDK**
   ```bash
   # Verify installation
   dotnet --version
   ```

2. **Download and Install SQL Server Express**
   - Download from: https://www.microsoft.com/en-us/sql-server/sql-server-editions-express
   - Run the installer
   - Use default settings or customize as needed

3. **Download SQL Server Management Studio (SSMS)**
   - Download from: https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
   - This tool helps manage your SQL Server databases

## Local Setup

### 1. Clone or Download the Project
```bash
git clone <repository-url>
cd BuildingCompanyApp
```

### 2. Create the Database

Open **SQL Server Management Studio** and run the script:

```sql
-- Open Data/database-schema.sql
-- Execute all commands to create the database and tables
```

Or use the command line:
```bash
sqlcmd -S localhost -i Data/database-schema.sql
```

### 3. Update Connection String

Edit `appsettings.json` and update the connection string if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local)\\SQLEXPRESS;Database=BuildingCompanyDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 4. Restore NuGet Packages
```bash
dotnet restore
```

### 5. Build the Application
```bash
dotnet build
```

### 6. Run the Application
```bash
dotnet run
```

Access the application at: `http://localhost:5000` or `https://localhost:5001`

### 7. Login

**Default Test Account:**
- Username: `admin`
- Password: `admin123`

(Note: You'll need to create the account first through the registration page)

## Application Usage

### Login & Registration
- New users can register via the registration page
- Passwords are securely hashed using BCrypt
- After login, users access the dashboard

### Dashboard
- View company metrics at a glance
- Quick access to all main sections
- Real-time project and worker statistics

### Workers Management
- View all workers with their profiles
- Add new workers with position, specialization, and contact info
- Edit worker information
- Delete worker profiles
- Upload profile pictures

### Projects Management
- Create new projects with budget, deadline, and client info
- Track project status (Planning, In Progress, On Hold, Completed, Cancelled)
- Assign workers to projects
- Upload project images and documents
- Monitor project deadlines

### Documents Management
- Upload documents (contracts, reports, certificates, plans)
- Organize documents by project or worker
- Set expiry dates for documents
- Download documents anytime
- Delete expired documents

## Deployment Guide

### Option 1: Deploy to Azure

#### Step 1: Create Azure Resources
1. Go to [Azure Portal](https://portal.azure.com)
2. Sign in or create a free account
3. Create a new **App Service Plan**
4. Create a new **App Service (Web App)**
5. Create a new **SQL Database** or **SQL Server**

#### Step 2: Publish Application
```bash
# From your project directory
dotnet publish -c Release -o ./publish

# Install Azure CLI
# Download from: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli

# Login to Azure
az login

# Create resource group
az group create --name BuildingCoRG --location eastus

# Create app service plan
az appservice plan create --name BuildingCoPlan --resource-group BuildingCoRG --sku B1

# Create web app
az webapp create --resource-group BuildingCoRG --plan BuildingCoPlan --name buildingco-app

# Deploy files
az webapp deployment source config-zip --resource-group BuildingCoRG --name buildingco-app --src publish.zip
```

#### Step 3: Configure Application Settings
In Azure Portal:
1. Go to your Web App → Configuration
2. Add Connection String:
   - Name: `DefaultConnection`
   - Value: Your SQL Database connection string
   - Type: `SQLServer`
3. Add Application Settings:
   - `ASPNETCORE_ENVIRONMENT`: `Production`

### Option 2: Deploy to AWS

#### Step 1: Create AWS Resources
1. Go to [AWS Management Console](https://aws.amazon.com/console/)
2. Create an **EC2 Instance** (Windows or Linux)
3. Create an **RDS Database** (SQL Server)
4. Create an **Elastic Beanstalk** application (optional)

#### Step 2: Prepare Application
```bash
# Build for production
dotnet publish -c Release

# Create deployment package
cd bin/Release/net7.0/publish
zip -r ../../../publish.zip .
```

#### Step 3: Deploy to Elastic Beanstalk
```bash
# Install EB CLI
pip install awsebcli

# Initialize
eb init -p "Windows Server 2019" BuildingCompanyApp

# Create environment
eb create buildingco-env

# Deploy
eb deploy
```

### Option 3: Deploy to DigitalOcean

#### Step 1: Create Droplet
1. Go to [DigitalOcean](https://www.digitalocean.com)
2. Create a new Droplet
3. Choose: App Platform → ASP.NET Core

#### Step 2: Deploy via GitHub
1. Connect your GitHub repository
2. Select deployment branch
3. DigitalOcean automatically builds and deploys

#### Step 3: Configure Database
1. Create a Managed Database (MySQL/PostgreSQL) or
2. Install SQL Server on the Droplet

### Option 4: Self-Hosted (VPS/Dedicated Server)

#### Step 1: Prepare Server
```bash
# SSH into your server
ssh user@your-server-ip

# Install .NET Runtime
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh

# Install SQL Server (if needed)
# Follow Microsoft installation guide for your OS
```

#### Step 2: Deploy Application
```bash
# Create app directory
mkdir -p /var/www/buildingco
cd /var/www/buildingco

# Upload your published application
scp -r ./publish/* user@server-ip:/var/www/buildingco/

# Create systemd service
sudo nano /etc/systemd/system/buildingco.service
```

Add:
```ini
[Unit]
Description=Building Company App
After=network.target

[Service]
Type=notify
User=www-data
WorkingDirectory=/var/www/buildingco
ExecStart=/usr/bin/dotnet /var/www/buildingco/BuildingCompanyApp.dll
Restart=on-failure
RestartSec=10

[Install]
WantedBy=multi-user.target
```

```bash
# Start service
sudo systemctl start buildingco
sudo systemctl enable buildingco

# Check status
sudo systemctl status buildingco
```

#### Step 3: Configure Web Server (Nginx)
```bash
# Install Nginx
sudo apt-get install nginx

# Create config
sudo nano /etc/nginx/sites-available/buildingco
```

Add:
```nginx
server {
    listen 80;
    server_name your-domain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

```bash
# Enable site
sudo ln -s /etc/nginx/sites-available/buildingco /etc/nginx/sites-enabled/

# Test and reload
sudo nginx -t
sudo systemctl reload nginx
```

## Domain and HTTPS Setup

### Getting a Domain

1. **Popular Domain Registrars:**
   - [GoDaddy](https://www.godaddy.com)
   - [Namecheap](https://www.namecheap.com)
   - [Google Domains](https://domains.google)
   - [HostGator](https://www.hostgator.com)

2. **Steps:**
   - Search for available domain
   - Register domain (usually $10-15/year)
   - Point nameservers to your hosting provider

### HTTPS/SSL Certificate

#### Option 1: Let's Encrypt (Free)
```bash
# Install Certbot
sudo apt-get install certbot python3-certbot-nginx

# Generate certificate
sudo certbot certonly --nginx -d your-domain.com

# Auto-renew
sudo certbot renew --dry-run
```

#### Option 2: AWS Certificate Manager (Free with AWS)
1. Go to AWS Certificate Manager
2. Request public certificate
3. Validate domain ownership
4. Use in CloudFront or ALB

#### Option 3: DigiCert/Comodo (Paid)
1. Purchase SSL certificate
2. Follow provider's installation instructions

## Environment Configuration

### Development
```json
{
  "ASPNETCORE_ENVIRONMENT": "Development",
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

### Production
```json
{
  "ASPNETCORE_ENVIRONMENT": "Production",
  "Logging": {
    "LogLevel": {
      "Default": "Error"
    }
  }
}
```

## Troubleshooting

### Database Connection Issues
```bash
# Check SQL Server is running
sqlcmd -S localhost -U sa

# Test connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
```

### Port Already in Use
```bash
# Change port in Properties/launchSettings.json
# Or kill process on port 5000
sudo lsof -ti:5000 | xargs kill -9
```

### Certificate Errors
```bash
# Trust development certificate
dotnet dev-certs https --trust
```

## Performance Optimization

1. **Database Indexing**: Already included in schema
2. **Caching**: Implement Redis for session/data caching
3. **CDN**: Use Azure CDN or CloudFlare for static assets
4. **Compression**: Enable gzip in Nginx
5. **Monitoring**: Use Azure Application Insights or Datadog

## Security Best Practices

1. ✅ Password hashing with BCrypt
2. ✅ SQL injection prevention with parameterized queries
3. ✅ HTTPS/SSL encryption
4. ✅ Session timeout (30 minutes)
5. ✅ Input validation on all forms
6. ✅ CORS protection
7. ✅ Rate limiting on authentication endpoints
8. ✅ Regular security updates

## Backup and Recovery

### Database Backup
```sql
-- Full backup
BACKUP DATABASE BuildingCompanyDB 
TO DISK = 'C:\Backups\BuildingCompanyDB.bak';

-- Restore
RESTORE DATABASE BuildingCompanyDB 
FROM DISK = 'C:\Backups\BuildingCompanyDB.bak';
```

### Application Backup
- Use version control (Git)
- Regular automated backups to cloud storage
- Document deployment steps

## Support and Maintenance

### Regular Maintenance Tasks
- Update .NET SDK monthly
- Monitor database performance
- Review and archive old documents
- Check error logs weekly
- Update security patches

### Getting Help
- [Microsoft Docs](https://docs.microsoft.com)
- [Stack Overflow](https://stackoverflow.com)
- [GitHub Issues](https://github.com)

## License

This project is licensed under the MIT License - see LICENSE file for details.

## Contributors

- Your Name (initial development)

## Changelog

### Version 1.0.0 (2024)
- Initial release
- Core features: Workers, Projects, Documents, Authentication
- Responsive design for all devices
- Deployment guides for major cloud providers
