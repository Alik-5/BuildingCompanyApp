# Complete Deployment Guide

## Table of Contents
1. [Pre-Deployment Requirements](#pre-deployment-requirements)
2. [Local Development Setup](#local-development-setup)
3. [Cloud Platform Deployments](#cloud-platform-deployments)
4. [Self-Hosted Deployments](#self-hosted-deployments)
5. [Domain & SSL Setup](#domain--ssl-setup)
6. [Post-Deployment Configuration](#post-deployment-configuration)

## Pre-Deployment Requirements

### Software & Tools Required

1. **.NET 7.0 SDK**
   - Download: https://dotnet.microsoft.com/download/dotnet/7.0
   - Verify: `dotnet --version`

2. **SQL Server**
   - SQL Server 2019+ or SQL Server Express
   - Download: https://www.microsoft.com/en-us/sql-server/sql-server-editions-express

3. **SQL Server Management Studio (SSMS)**
   - Download: https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms

4. **Visual Studio 2022** or **VS Code**
   - IDE for development

5. **Git**
   - Version control: https://git-scm.com/

### System Requirements

| Component | Minimum | Recommended |
|-----------|---------|------------|
| OS | Windows Server 2012+ | Windows Server 2019+ |
| RAM | 2 GB | 4+ GB |
| CPU | 2 cores | 4+ cores |
| Storage | 500 MB | 1+ GB |
| Database | SQL Express | SQL Server Standard+ |

## Local Development Setup

### Step 1: Install Dependencies

```bash
# Check .NET version
dotnet --version

# Download and install SQL Server Express if not already installed
# Download and install SSMS if not already installed
```

### Step 2: Clone/Download Project

```bash
# Clone from Git (if available)
git clone https://github.com/your-repo/BuildingCompanyApp.git
cd BuildingCompanyApp

# Or extract from ZIP
# Navigate to extracted folder
```

### Step 3: Create Database

**Using SSMS:**
1. Open SQL Server Management Studio
2. Connect to your SQL Server
3. Open file: `Data/database-schema.sql`
4. Execute the script (F5)

**Using PowerShell:**
```powershell
$connectionString = "Server=(local)\SQLEXPRESS;Integrated Security=true;"
sqlcmd -S "(local)\SQLEXPRESS" -i "Data\database-schema.sql"
```

### Step 4: Configure Application

Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local)\\SQLEXPRESS;Database=BuildingCompanyDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Step 5: Run Application

```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run
dotnet run

# Or in watch mode for development
dotnet watch run
```

Access at: `http://localhost:5000`

## Cloud Platform Deployments

### Azure App Service Deployment

#### Prerequisites
- Azure subscription (free or paid)
- Azure CLI installed
- Application published locally

#### Deployment Steps

```bash
# 1. Login to Azure
az login

# 2. Create resource group
az group create \
  --name BuildingCoRG \
  --location eastus

# 3. Create App Service Plan
az appservice plan create \
  --name BuildingCoPlan \
  --resource-group BuildingCoRG \
  --sku B1 \
  --is-linux

# 4. Create Web App
az webapp create \
  --resource-group BuildingCoRG \
  --plan BuildingCoPlan \
  --name buildingco-app \
  --runtime "DOTNET|7.0"

# 5. Configure connection string
az webapp config connection-string set \
  --resource-group BuildingCoRG \
  --name buildingco-app \
  --connection-string-type SQLServer \
  --settings DefaultConnection="your-connection-string"

# 6. Create SQL Database
az sql server create \
  --name buildingco-server \
  --resource-group BuildingCoRG \
  --admin-user admin123 \
  --admin-password YourPassword123!

az sql db create \
  --resource-group BuildingCoRG \
  --server buildingco-server \
  --name BuildingCompanyDB

# 7. Publish application
dotnet publish -c Release -o ./publish
cd publish
zip -r ../publish.zip .
cd ..

# 8. Deploy
az webapp deployment source config-zip \
  --resource-group BuildingCoRG \
  --name buildingco-app \
  --src publish.zip
```

#### Azure Portal Configuration

1. **Connection String:**
   - Go to: App Service → Configuration → Connection Strings
   - Add: `DefaultConnection` with SQL Database connection string

2. **Application Settings:**
   - Go to: App Service → Configuration → Application Settings
   - Add: `ASPNETCORE_ENVIRONMENT` = `Production`
   - Add: `WEBSITE_RUN_FROM_PACKAGE` = `1`

3. **SSL/HTTPS:**
   - Go to: App Service → Custom Domains
   - Add your domain and SSL certificate

### AWS Elastic Beanstalk Deployment

#### Prerequisites
- AWS account
- AWS CLI configured
- EB CLI installed

#### Deployment Steps

```bash
# 1. Initialize Elastic Beanstalk
eb init -p "IIS 10.0" BuildingCompanyApp --region us-east-1

# 2. Create environment
eb create buildingco-env

# 3. Configure environment
# Create .ebextensions/environment.config
```

File: `.ebextensions/environment.config`
```yaml
option_settings:
  aws:elasticbeanstalk:environment:proxy:
    ProxyServer: IIS
  aws:elasticbeanstalk:container:dotnet:image:
    IISVersion: 10.0
  aws:elasticbeanstalk:application:environment:
    ASPNETCORE_ENVIRONMENT: Production
```

```bash
# 4. Deploy
eb deploy

# 5. Set up database
aws rds create-db-instance \
  --db-instance-identifier buildingco-db \
  --db-instance-class db.t3.micro \
  --engine sqlserver-ex \
  --master-username admin \
  --master-user-password YourPassword123!

# 6. Configure environment variable
eb setenv DefaultConnection="your-connection-string"
```

### DigitalOcean App Platform

#### Steps via Web Console

1. **Create New App:**
   - DigitalOcean Dashboard → Create → App
   - Select GitHub/GitLab

2. **Choose Repository:**
   - Select your BuildingCompanyApp repo
   - Select deployment branch (main)

3. **Configure:**
   - Set builder: `buildpack`
   - Set run command: `dotnet BuildingCompanyApp.dll`

4. **Add Database:**
   - DigitalOcean → Create → Database
   - Select SQL Server
   - Note connection string

5. **Set Environment Variables:**
   - App → Settings
   - Add: `DefaultConnection` = your connection string
   - Add: `ASPNETCORE_ENVIRONMENT` = `Production`

6. **Deploy:**
   - Click "Deploy App"
   - Monitor deployment logs

### Heroku Deployment (For Testing)

```bash
# 1. Install Heroku CLI
# 2. Login
heroku login

# 3. Create app
heroku create buildingco-app

# 4. Add buildpack
heroku buildpacks:add heroku/dotnet

# 5. Set environment variables
heroku config:set ASPNETCORE_ENVIRONMENT=Production
heroku config:set DefaultConnection="your-connection-string"

# 6. Deploy
git push heroku main

# 7. View logs
heroku logs --tail
```

## Self-Hosted Deployments

### Linux VPS (Ubuntu 20.04+)

#### Step 1: Prepare Server

```bash
# SSH into server
ssh user@your-server-ip

# Update system
sudo apt update && sudo apt upgrade -y

# Install .NET Runtime
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version latest

# Add to PATH
echo 'export PATH=$PATH:/root/.dotnet' >> ~/.bashrc
source ~/.bashrc

# Verify installation
dotnet --version
```

#### Step 2: Install SQL Server

```bash
# Add Microsoft repository
wget -qO- https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
sudo add-apt-repository "$(wget -qO- https://packages.microsoft.com/config/ubuntu/20.04/mssql-server-2019.list)"

# Install SQL Server
sudo apt-get update
sudo apt-get install -y mssql-server

# Start service
sudo systemctl start mssql-server
sudo systemctl enable mssql-server

# Configure SA password
sudo /opt/mssql/bin/mssql-conf set-sa-password
```

#### Step 3: Deploy Application

```bash
# Create app directory
sudo mkdir -p /var/www/buildingco
cd /var/www/buildingco

# Upload files (from local machine)
scp -r ./publish/* user@server-ip:/var/www/buildingco/

# Create systemd service
sudo nano /etc/systemd/system/buildingco.service
```

Add:
```ini
[Unit]
Description=Building Company App
After=network.target mssql-server.service

[Service]
Type=notify
User=www-data
WorkingDirectory=/var/www/buildingco
ExecStart=/root/.dotnet/dotnet /var/www/buildingco/BuildingCompanyApp.dll
Restart=on-failure
RestartSec=10
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
```

```bash
# Enable and start service
sudo systemctl daemon-reload
sudo systemctl enable buildingco
sudo systemctl start buildingco

# Check status
sudo systemctl status buildingco

# View logs
sudo journalctl -u buildingco -f
```

#### Step 4: Configure Nginx Reverse Proxy

```bash
# Install Nginx
sudo apt-get install -y nginx

# Create config
sudo nano /etc/nginx/sites-available/buildingco
```

Add:
```nginx
server {
    listen 80;
    server_name your-domain.com;
    
    client_max_body_size 100M;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}
```

```bash
# Enable site
sudo ln -s /etc/nginx/sites-available/buildingco /etc/nginx/sites-enabled/

# Test configuration
sudo nginx -t

# Reload
sudo systemctl reload nginx
sudo systemctl enable nginx
```

### Windows Server Deployment

#### Step 1: Install Requirements

1. Download and install .NET 7.0 Hosting Bundle
2. Download and install SQL Server
3. Install IIS (Windows Features)

#### Step 2: Deploy Application

```powershell
# Create app directory
New-Item -ItemType Directory -Path "C:\inetpub\BuildingCoApp"

# Copy published files
Copy-Item -Path ".\publish\*" -Destination "C:\inetpub\BuildingCoApp" -Recurse
```

#### Step 3: Create IIS Application

1. Open IIS Manager
2. Right-click Sites → Add Website
3. Configure:
   - Site name: BuildingCompanyApp
   - Physical path: C:\inetpub\BuildingCoApp
   - Binding: your-domain.com (port 80)

#### Step 4: Configure Application Pool

1. Select application pool
2. Set to .NET CLR Version: No Managed Code
3. Set identity to: ApplicationPoolIdentity

## Domain & SSL Setup

### Register Domain

**Popular registrars:**
- GoDaddy: https://www.godaddy.com
- Namecheap: https://www.namecheap.com
- Google Domains: https://domains.google
- Bluehost: https://www.bluehost.com

**Steps:**
1. Search and register domain
2. Point nameservers to your hosting provider
3. Wait for DNS propagation (up to 48 hours)

### Get SSL Certificate

#### Option 1: Let's Encrypt (Free)

```bash
# Install Certbot
sudo apt-get install -y certbot python3-certbot-nginx

# Get certificate
sudo certbot certonly --nginx -d your-domain.com

# Setup auto-renewal
sudo certbot renew --dry-run

# Create renewal cron job (runs daily)
```

#### Option 2: AWS Certificate Manager (Free)

1. Go to AWS Certificate Manager
2. Request certificate
3. Verify domain ownership
4. Use in CloudFront or ALB

#### Option 3: Paid Certificates

- DigiCert, Comodo, GoDaddy SSL
- Follow provider's installation guide

### Configure HTTPS in Nginx

```nginx
server {
    listen 443 ssl http2;
    server_name your-domain.com;

    ssl_certificate /etc/letsencrypt/live/your-domain.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/your-domain.com/privkey.pem;
    
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;
    ssl_prefer_server_ciphers on;

    location / {
        proxy_pass http://localhost:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto https;
    }
}

# Redirect HTTP to HTTPS
server {
    listen 80;
    server_name your-domain.com;
    return 301 https://$server_name$request_uri;
}
```

## Post-Deployment Configuration

### Health Checks

```bash
# Test application
curl https://your-domain.com

# Check database connection
sqlcmd -S server-name -U sa -P password -Q "SELECT @@VERSION"

# Monitor logs
tail -f /var/log/buildingco.log
```

### Monitoring & Alerts

1. **Application Insights** (Azure):
   ```bash
   # Add to appsettings.json
   "ApplicationInsights": {
     "InstrumentationKey": "your-key"
   }
   ```

2. **CloudWatch** (AWS):
   - Set up alarm for CPU, Memory, Error rates

3. **Monitoring Services**:
   - Datadog: https://www.datadoghq.com
   - New Relic: https://newrelic.com
   - Sentry: https://sentry.io

### Backup Strategy

```bash
# Daily database backup
0 2 * * * /usr/bin/mysqldump -u root -p'password' BuildingCompanyDB > /backups/buildingco-$(date +\%Y\%m\%d).sql

# Upload to cloud
0 3 * * * aws s3 sync /backups s3://my-backup-bucket/buildingco/
```

### Performance Optimization

1. **Enable Caching:**
   ```nginx
   location ~* \.(jpg|jpeg|png|gif|ico|css|js|svg|woff|woff2)$ {
       expires 30d;
       add_header Cache-Control "public, immutable";
   }
   ```

2. **Enable Compression:**
   ```nginx
   gzip on;
   gzip_types text/plain application/json;
   gzip_min_length 1000;
   ```

3. **Database Optimization:**
   - Regular index maintenance
   - Query optimization
   - Connection pooling

## Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| "Connection refused" | Check if application is running |
| "Port 5000 already in use" | Kill process: `sudo lsof -ti:5000 \| xargs kill -9` |
| "Database connection failed" | Verify connection string and SQL Server is running |
| "SSL certificate error" | Renew certificate: `sudo certbot renew` |
| "Permission denied" | Check file permissions: `sudo chmod -R 755 /var/www/buildingco` |

### Logs Location

- **Linux**: `/var/log/buildingco.log`, `journalctl -u buildingco -f`
- **Windows**: Event Viewer → Windows Logs → Application
- **Docker**: `docker logs container-name`

## Support

For issues or questions:
- Check documentation
- Search Stack Overflow
- Open GitHub issue
- Contact cloud provider support
