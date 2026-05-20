# Qore Workflow And Deployment

## How Qore works

### Roles
- `Admin` and `Manager` can see all projects, all employees, all materials, all finance records, all CRM data, all documents, and all tasks.
- `Employee` can see only:
  - assigned projects
  - assigned tasks
  - personal salary and payroll records

### Manager workflow
1. Manager logs in.
2. Manager creates or edits employees and gives each employee a `username`, `email`, and `password`.
3. Manager creates a project.
4. Manager assigns employees to that project.
5. Manager creates tasks and assigns each task to a specific employee.
6. Employee logs in with the account created by the manager.
7. Employee sees only:
   - personal dashboard
   - own projects
   - own tasks
   - own salary page

### Employee workflow
1. Employee opens Qore login.
2. Employee signs in with the credentials created by management.
3. Qore automatically limits the navigation and data scope.
4. Employee updates task progress and latest comment from the task page.

## Demo accounts
- `admin / admin123`
- `karen.manager / karen123`
- `suren.worker / suren123`

## Where data is saved right now

Current Qore data is stored in:
- `Services/AppDataStore.cs`

This means:
- data is stored in memory while the app is running
- if the app restarts, new changes are lost
- this is fine for demo/prototype use
- this is not the final production storage model

## Recommended production storage

For real business use, Qore should save data in:
- SQL Server on the same machine, or
- SQL Server on another server, or
- Azure SQL if hosted in cloud

Recommended production-persisted data:
- users
- employees
- projects
- project assignments
- tasks
- materials
- material transactions
- finance entries
- invoices
- clients
- documents metadata

Uploaded files should be saved in:
- local server disk for a small deployment, or
- cloud/object storage for a larger deployment

## Install on another computer

### Option 1: Simple local install
1. Install `.NET 8 SDK` or `.NET 8 Hosting Bundle`.
2. Copy the Qore project folder.
3. Open terminal in the project folder.
4. Run:

```powershell
dotnet restore
dotnet build
dotnet run
```

5. Open the shown localhost address in browser.

### Option 2: Publish and run
1. On the source computer run:

```powershell
dotnet publish -c Release -o .\publish
```

2. Copy the `publish` folder to the target machine.
3. On the target machine run:

```powershell
.\BuildingCompanyApp.exe
```

## Install on a Windows server

### Recommended approach
1. Install `.NET Hosting Bundle`.
2. Publish Qore with `dotnet publish`.
3. Copy the publish output to the server.
4. Host it with:
   - IIS, or
   - a Windows service, or
   - reverse proxy + Kestrel

### IIS approach
1. Install IIS.
2. Install `.NET Hosting Bundle`.
3. Publish the app.
4. Create a new IIS site pointing to the `publish` folder.
5. Set application pool to `No Managed Code`.
6. Start the site.

## Important note before production

Right now Qore is functionally role-based, but storage is still demo-mode in-memory storage.

Before real deployment for a business, the next upgrade should be:
- replace `AppDataStore.cs` with SQL Server persistence
- keep uploaded files in a stable storage location
- configure backups
- set strong passwords
- add HTTPS
- optionally add audit logs
