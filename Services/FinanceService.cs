using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public interface IFinanceService
    {
        Task<List<FinanceEntry>> GetAllEntriesAsync();
        Task<List<Invoice>> GetAllInvoicesAsync();
        Task AddEntryAsync(FinanceEntry entry);
        Task AddInvoiceAsync(Invoice invoice);
    }

    public class FinanceService : IFinanceService
    {
        private readonly IFinanceRepository _financeRepository;

        public FinanceService(IFinanceRepository financeRepository)
        {
            _financeRepository = financeRepository;
        }

        public async Task<List<FinanceEntry>> GetAllEntriesAsync()
        {
            return await _financeRepository.GetAllEntriesAsync();
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return await _financeRepository.GetAllInvoicesAsync();
        }

        public async Task AddEntryAsync(FinanceEntry entry)
        {
            await _financeRepository.AddEntryAsync(entry);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _financeRepository.AddInvoiceAsync(invoice);
        }
    }
}
