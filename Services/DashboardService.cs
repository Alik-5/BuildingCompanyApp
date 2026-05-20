using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardMetricsAsync();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IFinanceRepository _financeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserContextService _userContextService;

        public DashboardService(
            IProjectRepository projectRepository,
            IWorkerRepository workerRepository,
            IMaterialRepository materialRepository,
            IFinanceRepository financeRepository,
            IClientRepository clientRepository,
            ITaskRepository taskRepository,
            IUserContextService userContextService)
        {
            _projectRepository = projectRepository;
            _workerRepository = workerRepository;
            _materialRepository = materialRepository;
            _financeRepository = financeRepository;
            _clientRepository = clientRepository;
            _taskRepository = taskRepository;
            _userContextService = userContextService;
        }

        public async Task<DashboardViewModel> GetDashboardMetricsAsync()
        {
            var currentUser = await _userContextService.GetCurrentUserAsync();
            var currentWorker = await _userContextService.GetCurrentWorkerAsync();
            var canManage = _userContextService.CanManageCompanyData();

            var allProjects = await _projectRepository.GetAllProjectsAsync();
            var allWorkers = await _workerRepository.GetAllWorkersAsync();
            var materials = await _materialRepository.GetAllMaterialsAsync();
            var financeEntries = await _financeRepository.GetAllEntriesAsync();
            var invoices = await _financeRepository.GetAllInvoicesAsync();
            var clients = await _clientRepository.GetAllClientsAsync();
            var tasks = await _taskRepository.GetAllTasksAsync();

            var scopedProjects = canManage
                ? allProjects
                : allProjects
                    .Where(project => currentWorker != null && project.ProjectWorkers.Any(link => link.WorkerId == currentWorker.WorkerId))
                    .ToList();

            var scopedTasks = canManage
                ? tasks
                : tasks.Where(task => currentWorker != null && task.AssignedWorkerId == currentWorker.WorkerId).ToList();

            var scopedFinanceEntries = canManage
                ? financeEntries
                : financeEntries.Where(entry => currentWorker != null && entry.WorkerId == currentWorker.WorkerId).ToList();

            var monthlyEntries = financeEntries
                .Where(entry => entry.EntryDate.Month == DateTime.Today.Month && entry.EntryDate.Year == DateTime.Today.Year)
                .ToList();

            var monthlyIncome = monthlyEntries.Where(entry => entry.EntryType == "Income").Sum(entry => entry.Amount);
            var monthlyExpenses = monthlyEntries.Where(entry => entry.EntryType == "Expense").Sum(entry => entry.Amount);

            return new DashboardViewModel
            {
                IsManagerView = canManage,
                UserDisplayName = currentWorker?.FullName ?? $"{currentUser?.FirstName} {currentUser?.LastName}".Trim(),
                RoleLabel = canManage ? "Management View" : "Employee View",
                TotalProjects = scopedProjects.Count,
                CompletedProjects = scopedProjects.Count(project => project.Status == "Completed"),
                ActiveProjects = scopedProjects.Count(project => project.Status == "In Progress"),
                TotalWorkers = canManage ? allWorkers.Count : 1,
                WorkersOnSite = canManage ? allWorkers.Count(worker => worker.AttendanceStatus is "Present" or "On Site") : (currentWorker?.AttendanceStatus is "Present" or "On Site" ? 1 : 0),
                TotalBudget = canManage ? allProjects.Sum(project => project.Budget) : scopedProjects.Sum(project => project.Budget),
                MonthlyIncome = canManage ? monthlyIncome : 0,
                MonthlyExpenses = canManage ? monthlyExpenses : scopedFinanceEntries.Sum(entry => entry.Amount),
                Profit = canManage ? monthlyIncome - monthlyExpenses : currentWorker?.Salary ?? 0,
                PersonalSalary = currentWorker?.Salary ?? 0,
                OpenTasks = scopedTasks.Count(task => task.Status != "Done" && task.Status != "Completed"),
                LowStockItems = materials.Count(material => material.QuantityInStock <= material.ReorderLevel),
                PendingInvoices = invoices.Count(invoice => invoice.Status == "Pending"),
                HighlightProjects = scopedProjects.Take(3).ToList(),
                UpcomingTasks = scopedTasks.OrderBy(task => task.DueDate).Take(4).ToList(),
                CriticalMaterials = materials
                    .Where(material => material.QuantityInStock <= material.ReorderLevel)
                    .OrderBy(material => material.QuantityInStock)
                    .Take(4)
                    .ToList(),
                RecentInvoices = canManage ? invoices.OrderBy(invoice => invoice.DueDate).Take(4).ToList() : new List<Invoice>(),
                ClientReminders = canManage ? clients.OrderBy(client => client.ReminderDate).Take(4).ToList() : new List<Client>(),
                RecentTransactions = scopedFinanceEntries.OrderByDescending(entry => entry.EntryDate).Take(5).ToList()
            };
        }
    }
}
