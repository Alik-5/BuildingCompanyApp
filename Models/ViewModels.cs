using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuildingCompanyApp.Models
{
    public class DashboardViewModel
    {
        public bool IsManagerView { get; set; }
        public string UserDisplayName { get; set; } = string.Empty;
        public string RoleLabel { get; set; } = string.Empty;
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int TotalWorkers { get; set; }
        public int WorkersOnSite { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyExpenses { get; set; }
        public decimal Profit { get; set; }
        public decimal PersonalSalary { get; set; }
        public int OpenTasks { get; set; }
        public int LowStockItems { get; set; }
        public int PendingInvoices { get; set; }
        public List<Project> HighlightProjects { get; set; } = new();
        public List<WorkTask> UpcomingTasks { get; set; } = new();
        public List<MaterialItem> CriticalMaterials { get; set; } = new();
        public List<Invoice> RecentInvoices { get; set; } = new();
        public List<Client> ClientReminders { get; set; } = new();
        public List<FinanceEntry> RecentTransactions { get; set; } = new();
    }

    public class DocumentHubViewModel
    {
        public List<Document> Documents { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
        public List<Worker> Workers { get; set; } = new();
    }

    public class WorkerOptionViewModel
    {
        public int WorkerId { get; set; }
        public string Label { get; set; } = string.Empty;
        public bool Selected { get; set; }
    }

    public class WorkerUpsertViewModel
    {
        public int WorkerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string AttendanceStatus { get; set; } = "Present";
        public decimal WorkHours { get; set; }
        public string ProfileImageUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AccessRole { get; set; } = "Employee";
        public bool HasExistingAccount { get; set; }
    }

    public class ProjectUpsertViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Planning";
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
        public string Location { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public string CurrentStage { get; set; } = "Foundation";
        public int StageCompletionPercent { get; set; }
        public string SiteManager { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public List<int> SelectedWorkerIds { get; set; } = new();
        public List<WorkerOptionViewModel> AvailableWorkers { get; set; } = new();
    }

    public class TaskBoardViewModel
    {
        public bool CanManage { get; set; }
        public string CurrentEmployeeName { get; set; } = string.Empty;
        public List<WorkTask> Tasks { get; set; } = new();
    }

    public class TaskUpsertViewModel
    {
        public int WorkTaskId { get; set; }
        public int ProjectId { get; set; }
        public int AssignedWorkerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Open";
        public int ProgressPercent { get; set; }
        public int CommentCount { get; set; }
        public string LatestComment { get; set; } = string.Empty;
        public bool NotificationsEnabled { get; set; }
        public DateTime DueDate { get; set; }
        public List<Project> AvailableProjects { get; set; } = new();
        public List<Worker> AvailableWorkers { get; set; } = new();
    }

    public class FinancePageViewModel
    {
        public bool CanManage { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public decimal SalaryPaidThisYear { get; set; }
        public List<FinanceEntry> Entries { get; set; } = new();
        public List<Invoice> Invoices { get; set; } = new();
    }
}
