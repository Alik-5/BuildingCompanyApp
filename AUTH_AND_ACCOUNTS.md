# Qore Authentication & Account Management Guide

## How Qore Authentication Works

### Current Demo Accounts (Already Seeded)

```
ADMIN ACCOUNT:
  Username: admin
  Password: admin123
  Role: Admin (can see everything, manage system)

MANAGER ACCOUNT:
  Username: karen.manager
  Password: karen123
  Role: Manager (can see all projects, employees, finance, create tasks)

EMPLOYEE ACCOUNTS:
  arman.engineer / arman123        → Role: Employee
  mariam.architect / mariam123     → Role: Employee
  suren.worker / suren123          → Role: Employee
  lilit.accountant / lilit123      → Role: Employee
```

---

## Authentication Flow Explained

### 1. **Login Process**
```
User enters Username & Password
          ↓
AuthController.Login()
          ↓
AuthService.AuthenticateAsync()
          ↓
Checks if user exists in AppDataStore.Users
          ↓
Compares password (BCrypt hash)
          ↓
If valid: stores UserId, Username, Role in Session
          ↓
Redirects to Dashboard
```

### 2. **Registration Process**
```
User fills Register form
          ↓
AuthController.Register()
          ↓
AuthService.RegisterAsync()
          ↓
Creates new User with:
  - Role = "Worker" (default)
  - Password hashed with BCrypt
  - IsActive = true
          ↓
User can now log in
```

### 3. **Role-Based Access**
```
After login, Qore checks user's Role:

ADMIN:
  ✓ Sees ALL projects, employees, materials, finance, CRM, tasks
  ✓ Can create/edit/delete employees
  ✓ Can create tasks for any employee
  ✓ Can manage all materials and finance
  ✓ Full system access

MANAGER:
  ✓ Sees ALL projects, employees, materials, finance, CRM, tasks
  ✓ Can create/edit employees
  ✓ Can create tasks for employees
  ✓ Can manage materials and finance
  ✓ Similar to Admin, but created explicitly

EMPLOYEE:
  ✓ Sees ONLY their own assigned projects
  ✓ Sees ONLY their own assigned tasks
  ✓ Sees ONLY their own salary/payroll info
  ✓ Can view documents related to their projects
  ✓ Limited read-only access
```

---

## The Workflow: How Manager Adds Task for Employee

### Step 1: Admin/Manager Creates Employee Account
```
1. Log in as Admin (admin / admin123) or Manager (karen.manager / karen123)
2. Go to "Team" section
3. Click "Add Employee"
4. Fill in:
   - First Name: "John"
   - Last Name: "Doe"
   - Email: "john.doe@qore.am"
   - Username: "john.doe"
   - Password: "securepassword123"
   - Role: "Worker" or "Engineer" or "Architect"
5. Click "Create Employee"
```

**What happens:**
- A new **User** account is created with Role = "Employee" or specified role
- A new **Worker** record is created (for HR, salary tracking)
- Email is sent to John (optional feature to implement)
- John can now log in with username/password

---

### Step 2: Manager Assigns Employee to Project
```
1. Go to "Projects" section
2. Open the project (e.g., "North Residence")
3. Click "Add Team Member"
4. Select "John Doe" from the employee list
5. Click "Assign"
```

**What happens:**
- John is now assigned to the project
- When John logs in, he will see this project in his dashboard

---

### Step 3: Manager Creates Task for Employee
```
1. Go to "Tasks" section
2. Click "Create New Task"
3. Fill in:
   - Title: "Complete foundation inspection"
   - Description: "Inspect concrete strength and alignment"
   - Project: "North Residence"
   - Assign To: "John Doe"
   - Priority: "High"
   - Due Date: "May 25, 2026"
4. Click "Create Task"
```

**What happens:**
- Task is created and assigned to John
- **John receives notification** (if enabled)
- When John logs in, he sees this task in his dashboard

---

### Step 4: Employee Logs In & Views Task
```
Employee (John) logs in:
  Username: john.doe
  Password: securepassword123
          ↓
John sees his personal dashboard:
  - His assigned projects
  - His assigned tasks
  - His salary/payroll info
  - Documents related to his projects
          ↓
John clicks on the task "Complete foundation inspection"
          ↓
John can:
  ✓ View task details and description
  ✓ See due date and priority
  ✓ Add comments to the task
  ✓ Update task status (Open → In Progress → Completed)
  ✓ Update progress percentage
```

---

## How to Create New Manager/Admin Accounts

### **Method 1: Register via UI (Becomes Employee)**
```
1. Go to http://localhost:5000 (or your app URL)
2. Click "Need an account? Create one"
3. Fill in registration form
4. New user gets Role = "Employee" by default
5. Can log in, but limited access
```

### **Method 2: Direct Database/Code Edit (Recommended for Production)**

Edit `Services/AppDataStore.cs` and add new users in the `Users` list:

```csharp
public static List<User> Users { get; } =
[
    // ... existing users ...
    
    // ADD YOUR NEW MANAGER HERE:
    CreateUser(7, "david.manager", "david@qore.am", "David", "Hakobyan", "Manager", "david789"),
    
    // OR ADD YOUR NEW ADMIN HERE:
    CreateUser(8, "admin2", "admin2@qore.am", "Qore", "Admin2", "Admin", "admin456"),
];

// Helper function at the bottom of the file:
private static User CreateUser(int id, string username, string email, string firstName, string lastName, string role, string password)
{
    return new User
    {
        UserId = id,
        Username = username,
        Email = email,
        FirstName = firstName,
        LastName = lastName,
        Role = role,  // "Admin", "Manager", or "Employee"
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        IsActive = true
    };
}
```

Then rebuild and run:
```powershell
dotnet build
dotnet run
```

Your new manager/admin account is immediately available!

---

## Adding a Proper Admin Panel (Future Enhancement)

Instead of editing code, we could add an Admin Panel where current Admins can:
- Create new Manager/Admin accounts
- Deactivate/activate users
- Reset passwords
- Change user roles

This would involve:
1. New route: `/Admin/CreateUser`
2. UI form with role selector dropdown
3. Authorization check (only Admin can access)
4. Database persistence (once we move from AppDataStore)

---

## Data Storage: Where Does Everything Live?

### **Currently (In-Memory)**
- All data stored in `AppDataStore.cs`
- Data is **lost on app restart**
- Good for demo, bad for production

### **Structure:**
```
AppDataStore.cs
  ├── Users[] (login accounts)
  ├── Workers[] (HR / personnel records)
  ├── Projects[] (construction projects)
  ├── Tasks[] (work assignments)
  ├── Materials[] (inventory)
  ├── Finance[] (income/expense)
  ├── Clients[] (CRM)
  ├── Documents[] (file storage metadata)
  └── Invoices[] (billing)
```

### **For Production (Next Step)**
Move from AppDataStore to SQL Server:
```
SQL Server Database
  ├── Users table (logins)
  ├── Workers table (HR)
  ├── Projects table
  ├── WorkTasks table
  ├── Materials table
  ├── FinanceEntries table
  ├── Clients table
  ├── Documents table
  └── Invoices table
```

This requires:
- Creating migration scripts
- Updating Repository classes to use EF Core
- Deploying to SQL Server

---

## Important Security Notes

1. **Demo Passwords**: Change these before production deployment
2. **BCrypt Hashing**: All passwords are hashed, never stored in plain text
3. **Session-Based**: Users stay logged in via Session cookies
4. **Role-Based Access**: Every controller checks user role before allowing actions
5. **No Password Reset**: Need to add email-based password reset (future feature)

---

## Quick Start: Create Your First Manager Account

1. Open `Services/AppDataStore.cs`
2. Find the `Users` list (line ~13)
3. Add a new line:
   ```csharp
   CreateUser(7, "mymanager", "my@email.com", "My", "Name", "Manager", "mypassword123"),
   ```
4. Save the file
5. Run `dotnet build` then `dotnet run`
6. Log in with: **mymanager** / **mypassword123**

That's it! You're now a Manager in Qore. 🚀

