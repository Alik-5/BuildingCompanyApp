using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public interface IWorkerService
    {
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker?> GetWorkerByIdAsync(int id);
        Task AddWorkerAsync(Worker worker);
        Task UpdateWorkerAsync(Worker worker);
        Task DeleteWorkerAsync(int id);
    }

    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;

        public WorkerService(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            return await _workerRepository.GetAllWorkersAsync();
        }

        public async Task<Worker?> GetWorkerByIdAsync(int id)
        {
            return await _workerRepository.GetWorkerByIdAsync(id);
        }

        public async Task AddWorkerAsync(Worker worker)
        {
            await _workerRepository.AddWorkerAsync(worker);
        }

        public async Task UpdateWorkerAsync(Worker worker)
        {
            await _workerRepository.UpdateWorkerAsync(worker);
        }

        public async Task DeleteWorkerAsync(int id)
        {
            await _workerRepository.DeleteWorkerAsync(id);
        }
    }
}
