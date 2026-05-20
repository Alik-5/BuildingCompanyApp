# QUICK START GUIDE - Building Company Management System

## For Development

### 1. Open Terminal/Command Prompt in project folder

### 2. Restore packages
```bash
dotnet restore
```

### 3. Build project
```bash
dotnet build
```

### 4. Run application
```bash
dotnet run
```

### 5. Open browser
Navigate to: `http://localhost:5000`

## For Deployment

### Azure
1. Go to Azure Portal
2. Create App Service
3. Create SQL Database
4. Use deployment section to publish
5. Update connection string in Application Settings

### DigitalOcean
1. Connect GitHub repo
2. Select ASP.NET Core
3. Configure environment variables
4. Deploy automatically

### AWS
```bash
# Install AWS CLI
# Run deployment script from your CI/CD pipeline
```

### Self-Hosted
```bash
# SSH to server
# Install .NET Runtime
# Upload files
# Configure systemd service
# Start service
```

## Troubleshooting

**Port 5000 already in use?**
```bash
sudo lsof -i :5000
kill -9 <PID>
```

**Database connection error?**
- Check connection string in appsettings.json
- Verify SQL Server is running
- Test connection with SSMS

**Certificate error?**
```bash
dotnet dev-certs https --trust
```

## Common Commands

```bash
# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Build for production
dotnet publish -c Release

# Run tests
dotnet test

# Clean build
dotnet clean && dotnet build
```
