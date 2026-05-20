-- Building Company Database Schema
-- MSSQL Server Script

-- Create Users (Authentication)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Role NVARCHAR(20) NOT NULL DEFAULT 'Worker', -- Admin, Manager, Worker
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- Create Workers
CREATE TABLE Workers (
    WorkerId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Position NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(200),
    Specialization NVARCHAR(200),
    HireDate DATETIME DEFAULT GETDATE(),
    Salary DECIMAL(10, 2),
    ProfileImageUrl NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- Create Projects
CREATE TABLE Projects (
    ProjectId INT PRIMARY KEY IDENTITY(1,1),
    ProjectName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Status NVARCHAR(50) NOT NULL DEFAULT 'Planning', -- Planning, In Progress, On Hold, Completed, Cancelled
    StartDate DATETIME NOT NULL,
    Deadline DATETIME NOT NULL,
    CompletionDate DATETIME NULL,
    Budget DECIMAL(15, 2),
    Location NVARCHAR(200),
    ClientName NVARCHAR(200),
    ClientEmail NVARCHAR(100),
    ClientPhone NVARCHAR(20),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE()
);

-- Create Project Workers (Many-to-Many relationship)
CREATE TABLE ProjectWorkers (
    ProjectWorkerId INT PRIMARY KEY IDENTITY(1,1),
    ProjectId INT NOT NULL,
    WorkerId INT NOT NULL,
    Role NVARCHAR(100),
    AssignedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId) ON DELETE CASCADE,
    FOREIGN KEY (WorkerId) REFERENCES Workers(WorkerId) ON DELETE CASCADE,
    UNIQUE(ProjectId, WorkerId)
);

-- Create Project Images
CREATE TABLE ProjectImages (
    ImageId INT PRIMARY KEY IDENTITY(1,1),
    ProjectId INT NOT NULL,
    ImageUrl NVARCHAR(MAX) NOT NULL,
    ImageName NVARCHAR(200),
    UploadedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId) ON DELETE CASCADE
);

-- Create Documents
CREATE TABLE Documents (
    DocumentId INT PRIMARY KEY IDENTITY(1,1),
    ProjectId INT,
    WorkerId INT,
    DocumentName NVARCHAR(200) NOT NULL,
    DocumentPath NVARCHAR(MAX) NOT NULL,
    DocumentType NVARCHAR(50), -- Contract, Report, Plan, Certificate, etc.
    UploadedBy INT NOT NULL,
    UploadedDate DATETIME DEFAULT GETDATE(),
    ExpiryDate DATETIME NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId) ON DELETE SET NULL,
    FOREIGN KEY (WorkerId) REFERENCES Workers(WorkerId) ON DELETE SET NULL,
    FOREIGN KEY (UploadedBy) REFERENCES Users(UserId)
);

-- Create Dashboard Metrics (for analytics)
CREATE TABLE DashboardMetrics (
    MetricId INT PRIMARY KEY IDENTITY(1,1),
    TotalProjects INT DEFAULT 0,
    CompletedProjects INT DEFAULT 0,
    ActiveProjects INT DEFAULT 0,
    TotalWorkers INT DEFAULT 0,
    TotalBudget DECIMAL(15, 2) DEFAULT 0,
    SpentBudget DECIMAL(15, 2) DEFAULT 0,
    LastUpdated DATETIME DEFAULT GETDATE()
);

-- Create Indexes for better performance
CREATE INDEX idx_Users_Username ON Users(Username);
CREATE INDEX idx_Users_Email ON Users(Email);
CREATE INDEX idx_Workers_UserId ON Workers(UserId);
CREATE INDEX idx_Projects_Status ON Projects(Status);
CREATE INDEX idx_Projects_Deadline ON Projects(Deadline);
CREATE INDEX idx_ProjectWorkers_ProjectId ON ProjectWorkers(ProjectId);
CREATE INDEX idx_ProjectWorkers_WorkerId ON ProjectWorkers(WorkerId);
CREATE INDEX idx_ProjectImages_ProjectId ON ProjectImages(ProjectId);
CREATE INDEX idx_Documents_ProjectId ON Documents(ProjectId);
CREATE INDEX idx_Documents_WorkerId ON Documents(WorkerId);
