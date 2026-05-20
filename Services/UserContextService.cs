using BuildingCompanyApp.Models;
using Microsoft.AspNetCore.Http;

namespace BuildingCompanyApp.Services
{
    public interface IUserContextService
    {
        bool IsAuthenticated();
        int? GetUserId();
        string GetRole();
        bool CanManageCompanyData();
        Task<User?> GetCurrentUserAsync();
        Task<Worker?> GetCurrentWorkerAsync();
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IWorkerRepository _workerRepository;

        public UserContextService(
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IWorkerRepository workerRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _workerRepository = workerRepository;
        }

        public bool IsAuthenticated()
        {
            return GetUserId().HasValue;
        }

        public int? GetUserId()
        {
            var rawUserId = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            return int.TryParse(rawUserId, out var userId) ? userId : null;
        }

        public string GetRole()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("Role") ?? string.Empty;
        }

        public bool CanManageCompanyData()
        {
            var role = GetRole();
            return role is "Admin" or "Manager";
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return null;
            }

            return await _userRepository.GetUserByIdAsync(userId.Value);
        }

        public async Task<Worker?> GetCurrentWorkerAsync()
        {
            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return null;
            }

            var workers = await _workerRepository.GetAllWorkersAsync();
            return workers.FirstOrDefault(worker => worker.UserId == userId.Value);
        }
    }
}
