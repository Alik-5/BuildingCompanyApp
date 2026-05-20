using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public interface IMaterialService
    {
        Task<List<MaterialItem>> GetAllMaterialsAsync();
        Task<List<MaterialTransaction>> GetRecentTransactionsAsync();
        Task AddMaterialAsync(MaterialItem material);
        Task AddTransactionAsync(MaterialTransaction transaction);
    }

    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<List<MaterialItem>> GetAllMaterialsAsync()
        {
            return await _materialRepository.GetAllMaterialsAsync();
        }

        public async Task<List<MaterialTransaction>> GetRecentTransactionsAsync()
        {
            return await _materialRepository.GetRecentTransactionsAsync();
        }

        public async Task AddMaterialAsync(MaterialItem material)
        {
            await _materialRepository.AddMaterialAsync(material);
        }

        public async Task AddTransactionAsync(MaterialTransaction transaction)
        {
            await _materialRepository.AddTransactionAsync(transaction);
        }
    }
}
