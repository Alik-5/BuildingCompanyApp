using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public class UserRepository : IUserRepository
    {
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await Task.FromResult(AppDataStore.Users.FirstOrDefault(u => u.Username == username));
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await Task.FromResult(AppDataStore.Users.FirstOrDefault(u => u.UserId == id));
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(AppDataStore.Users.OrderBy(u => u.Username).ToList());
        }

        public async Task AddUserAsync(User user)
        {
            user.UserId = AppDataStore.Users.Count == 0 ? 1 : AppDataStore.Users.Max(u => u.UserId) + 1;
            user.CreatedDate = DateTime.UtcNow;
            AppDataStore.Users.Add(user);
            await Task.CompletedTask;
        }

        public async Task UpdateUserAsync(User user)
        {
            var existing = AppDataStore.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (existing != null)
            {
                AppDataStore.Users.Remove(existing);
                AppDataStore.Users.Add(user);
            }
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = AppDataStore.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
                AppDataStore.Users.Remove(user);
            await Task.CompletedTask;
        }
    }

    public class WorkerRepository : IWorkerRepository
    {
        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            return await Task.FromResult(AppDataStore.Workers.OrderBy(w => w.FullName).ToList());
        }

        public async Task<Worker?> GetWorkerByIdAsync(int id)
        {
            return await Task.FromResult(AppDataStore.Workers.FirstOrDefault(w => w.WorkerId == id));
        }

        public async Task AddWorkerAsync(Worker worker)
        {
            worker.WorkerId = AppDataStore.Workers.Count == 0 ? 1 : AppDataStore.Workers.Max(w => w.WorkerId) + 1;
            worker.CreatedDate = DateTime.UtcNow;
            AppDataStore.Workers.Add(worker);
            await Task.CompletedTask;
        }

        public async Task UpdateWorkerAsync(Worker worker)
        {
            var existing = AppDataStore.Workers.FirstOrDefault(w => w.WorkerId == worker.WorkerId);
            if (existing != null)
            {
                AppDataStore.Workers.Remove(existing);
                AppDataStore.Workers.Add(worker);
            }
            await Task.CompletedTask;
        }

        public async Task DeleteWorkerAsync(int id)
        {
            var worker = AppDataStore.Workers.FirstOrDefault(w => w.WorkerId == id);
            if (worker != null)
                AppDataStore.Workers.Remove(worker);
            await Task.CompletedTask;
        }
    }

    public class ProjectRepository : IProjectRepository
    {
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            foreach (var project in AppDataStore.Projects)
            {
                project.Documents = AppDataStore.Documents.Where(d => d.ProjectId == project.ProjectId).ToList();
            }

            return await Task.FromResult(AppDataStore.Projects.OrderByDescending(p => p.UpdatedDate).ToList());
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            var project = AppDataStore.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project != null)
            {
                project.Documents = AppDataStore.Documents.Where(d => d.ProjectId == id).ToList();
            }

            return await Task.FromResult(project);
        }

        public async Task AddProjectAsync(Project project)
        {
            project.ProjectId = AppDataStore.Projects.Count == 0 ? 1 : AppDataStore.Projects.Max(p => p.ProjectId) + 1;
            project.Stages = BuildStages(project.ProjectId, project.CurrentStage, project.StageCompletionPercent);
            AppDataStore.Projects.Add(project);
            await Task.CompletedTask;
        }

        public async Task UpdateProjectAsync(Project project)
        {
            var existing = AppDataStore.Projects.FirstOrDefault(p => p.ProjectId == project.ProjectId);
            if (existing != null)
            {
                project.Stages = BuildStages(project.ProjectId, project.CurrentStage, project.StageCompletionPercent);
                AppDataStore.Projects.Remove(existing);
                AppDataStore.Projects.Add(project);
            }
            await Task.CompletedTask;
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = AppDataStore.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project != null)
            {
                AppDataStore.Projects.Remove(project);
            }
            await Task.CompletedTask;
        }

        public async Task<List<Project>> GetProjectsByStatusAsync(string status)
        {
            return await Task.FromResult(AppDataStore.Projects.Where(p => p.Status == status).ToList());
        }

        public async Task ReplaceProjectAssignmentsAsync(int projectId, IEnumerable<int> workerIds)
        {
            AppDataStore.ReplaceProjectAssignments(projectId, workerIds);
            await Task.CompletedTask;
        }

        private static List<ProjectStage> BuildStages(int projectId, string currentStage, int stagePercent)
        {
            var stageNames = new[] { "Foundation", "Walls", "Roof", "Interior", "Finished" };
            var currentIndex = Array.IndexOf(stageNames, currentStage);
            if (currentIndex < 0)
            {
                currentIndex = 0;
            }

            return stageNames
                .Select((stageName, index) => new ProjectStage
                {
                    ProjectId = projectId,
                    ProjectStageId = (projectId * 10) + index,
                    StageName = stageName,
                    DisplayOrder = index + 1,
                    IsCompleted = index < currentIndex || (index == currentIndex && stagePercent >= 100)
                })
                .ToList();
        }
    }

    public class DocumentRepository : IDocumentRepository
    {
        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            foreach (var document in AppDataStore.Documents)
            {
                document.Project = document.ProjectId.HasValue
                    ? AppDataStore.Projects.FirstOrDefault(p => p.ProjectId == document.ProjectId.Value)
                    : null;
                document.Worker = document.WorkerId.HasValue
                    ? AppDataStore.Workers.FirstOrDefault(w => w.WorkerId == document.WorkerId.Value)
                    : null;
            }

            return await Task.FromResult(AppDataStore.Documents.OrderByDescending(d => d.UploadedDate).ToList());
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            return await Task.FromResult(AppDataStore.Documents.FirstOrDefault(d => d.DocumentId == id));
        }

        public async Task AddDocumentAsync(Document document)
        {
            document.DocumentId = AppDataStore.Documents.Count == 0 ? 1 : AppDataStore.Documents.Max(d => d.DocumentId) + 1;
            AppDataStore.Documents.Add(document);
            await Task.CompletedTask;
        }

        public async Task DeleteDocumentAsync(int id)
        {
            var document = AppDataStore.Documents.FirstOrDefault(d => d.DocumentId == id);
            if (document != null)
                AppDataStore.Documents.Remove(document);
            await Task.CompletedTask;
        }

        public async Task<List<Document>> GetDocumentsByProjectAsync(int projectId)
        {
            return await Task.FromResult(AppDataStore.Documents.Where(d => d.ProjectId == projectId).ToList());
        }
    }

    public class MaterialRepository : IMaterialRepository
    {
        public async Task<List<MaterialItem>> GetAllMaterialsAsync()
        {
            foreach (var material in AppDataStore.Materials)
            {
                material.Transactions = AppDataStore.MaterialTransactions
                    .Where(t => t.MaterialItemId == material.MaterialItemId)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToList();
            }

            return await Task.FromResult(AppDataStore.Materials.OrderBy(m => m.Name).ToList());
        }

        public async Task AddMaterialAsync(MaterialItem material)
        {
            material.MaterialItemId = AppDataStore.Materials.Count == 0 ? 1 : AppDataStore.Materials.Max(m => m.MaterialItemId) + 1;
            material.LastUpdated = DateTime.Today;
            AppDataStore.Materials.Add(material);
            await Task.CompletedTask;
        }

        public async Task AddTransactionAsync(MaterialTransaction transaction)
        {
            transaction.MaterialTransactionId = AppDataStore.MaterialTransactions.Count == 0 ? 1 : AppDataStore.MaterialTransactions.Max(t => t.MaterialTransactionId) + 1;
            transaction.TransactionDate = transaction.TransactionDate == default ? DateTime.Today : transaction.TransactionDate;
            AppDataStore.MaterialTransactions.Add(transaction);

            var material = AppDataStore.Materials.FirstOrDefault(m => m.MaterialItemId == transaction.MaterialItemId);
            if (material != null)
            {
                var direction = string.Equals(transaction.TransactionType, "OUT", StringComparison.OrdinalIgnoreCase) ? -1 : 1;
                material.QuantityInStock += direction * transaction.Quantity;
                material.LastUpdated = transaction.TransactionDate;
            }

            await Task.CompletedTask;
        }

        public async Task<List<MaterialTransaction>> GetRecentTransactionsAsync()
        {
            return await Task.FromResult(AppDataStore.MaterialTransactions.OrderByDescending(t => t.TransactionDate).ToList());
        }
    }

    public class FinanceRepository : IFinanceRepository
    {
        public async Task<List<FinanceEntry>> GetAllEntriesAsync()
        {
            return await Task.FromResult(AppDataStore.FinanceEntries.OrderByDescending(e => e.EntryDate).ToList());
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return await Task.FromResult(AppDataStore.Invoices.OrderBy(i => i.DueDate).ToList());
        }

        public async Task AddEntryAsync(FinanceEntry entry)
        {
            entry.FinanceEntryId = AppDataStore.FinanceEntries.Count == 0 ? 1 : AppDataStore.FinanceEntries.Max(e => e.FinanceEntryId) + 1;
            AppDataStore.FinanceEntries.Add(entry);
            await Task.CompletedTask;
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            invoice.InvoiceId = AppDataStore.Invoices.Count == 0 ? 1 : AppDataStore.Invoices.Max(i => i.InvoiceId) + 1;
            AppDataStore.Invoices.Add(invoice);
            await Task.CompletedTask;
        }
    }

    public class ClientRepository : IClientRepository
    {
        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await Task.FromResult(AppDataStore.Clients.OrderBy(c => c.ReminderDate).ToList());
        }

        public async Task AddClientAsync(Client client)
        {
            client.ClientId = AppDataStore.Clients.Count == 0 ? 1 : AppDataStore.Clients.Max(c => c.ClientId) + 1;
            AppDataStore.Clients.Add(client);
            await Task.CompletedTask;
        }
    }

    public class TaskRepository : ITaskRepository
    {
        public async Task<List<WorkTask>> GetAllTasksAsync()
        {
            return await Task.FromResult(AppDataStore.Tasks.OrderBy(t => t.DueDate).ToList());
        }

        public async Task AddTaskAsync(WorkTask task)
        {
            task.WorkTaskId = AppDataStore.Tasks.Count == 0 ? 1 : AppDataStore.Tasks.Max(t => t.WorkTaskId) + 1;
            AppDataStore.Tasks.Add(task);
            await Task.CompletedTask;
        }

        public async Task<WorkTask?> GetTaskByIdAsync(int id)
        {
            return await Task.FromResult(AppDataStore.Tasks.FirstOrDefault(task => task.WorkTaskId == id));
        }

        public async Task UpdateTaskAsync(WorkTask task)
        {
            var existing = AppDataStore.Tasks.FirstOrDefault(item => item.WorkTaskId == task.WorkTaskId);
            if (existing != null)
            {
                AppDataStore.Tasks.Remove(existing);
                AppDataStore.Tasks.Add(task);
            }

            await Task.CompletedTask;
        }
    }
}
