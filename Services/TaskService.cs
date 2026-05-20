using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public interface ITaskService
    {
        Task<List<WorkTask>> GetAllTasksAsync();
        Task AddTaskAsync(WorkTask task);
        Task<WorkTask?> GetTaskByIdAsync(int id);
        Task UpdateTaskAsync(WorkTask task);
    }

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<WorkTask>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }

        public async Task AddTaskAsync(WorkTask task)
        {
            await _taskRepository.AddTaskAsync(task);
        }

        public async Task<WorkTask?> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task UpdateTaskAsync(WorkTask task)
        {
            await _taskRepository.UpdateTaskAsync(task);
        }
    }
}
