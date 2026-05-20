using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public interface IClientService
    {
        Task<List<Client>> GetAllClientsAsync();
        Task AddClientAsync(Client client);
    }

    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _clientRepository.GetAllClientsAsync();
        }

        public async Task AddClientAsync(Client client)
        {
            await _clientRepository.AddClientAsync(client);
        }
    }
}
