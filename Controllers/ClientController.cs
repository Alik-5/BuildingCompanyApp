using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingCompanyApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IUserContextService _userContextService;

        public ClientController(IClientService clientService, IUserContextService userContextService)
        {
            _clientService = clientService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(await _clientService.GetAllClientsAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                client.LastContactDate = client.LastContactDate == default ? DateTime.Today : client.LastContactDate;
                client.ReminderDate = client.ReminderDate == default ? DateTime.Today.AddDays(7) : client.ReminderDate;
                await _clientService.AddClientAsync(client);
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }
    }
}
