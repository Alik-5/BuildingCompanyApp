using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }

    public interface IWorkerRepository
    {
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker?> GetWorkerByIdAsync(int id);
        Task AddWorkerAsync(Worker worker);
        Task UpdateWorkerAsync(Worker worker);
        Task DeleteWorkerAsync(int id);
    }

    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task<List<Project>> GetProjectsByStatusAsync(string status);
        Task ReplaceProjectAssignmentsAsync(int projectId, IEnumerable<int> workerIds);
    }

    public interface IDocumentRepository
    {
        Task<List<Document>> GetAllDocumentsAsync();
        Task<Document?> GetDocumentByIdAsync(int id);
        Task AddDocumentAsync(Document document);
        Task DeleteDocumentAsync(int id);
        Task<List<Document>> GetDocumentsByProjectAsync(int projectId);
    }

    public interface IMaterialRepository
    {
        Task<List<MaterialItem>> GetAllMaterialsAsync();
        Task AddMaterialAsync(MaterialItem material);
        Task AddTransactionAsync(MaterialTransaction transaction);
        Task<List<MaterialTransaction>> GetRecentTransactionsAsync();
    }

    public interface IFinanceRepository
    {
        Task<List<FinanceEntry>> GetAllEntriesAsync();
        Task<List<Invoice>> GetAllInvoicesAsync();
        Task AddEntryAsync(FinanceEntry entry);
        Task AddInvoiceAsync(Invoice invoice);
    }

    public interface IClientRepository
    {
        Task<List<Client>> GetAllClientsAsync();
        Task AddClientAsync(Client client);
    }

    public interface ITaskRepository
    {
        Task<List<WorkTask>> GetAllTasksAsync();
        Task AddTaskAsync(WorkTask task);
        Task<WorkTask?> GetTaskByIdAsync(int id);
        Task UpdateTaskAsync(WorkTask task);
    }
}
