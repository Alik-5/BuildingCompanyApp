using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingCompanyApp.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;
        private readonly IUserContextService _userContextService;

        public MaterialController(IMaterialService materialService, IUserContextService userContextService)
        {
            _materialService = materialService;
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

            ViewBag.Transactions = await _materialService.GetRecentTransactionsAsync();
            return View(await _materialService.GetAllMaterialsAsync());
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
        public async Task<IActionResult> Create(MaterialItem material)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                material.LastUpdated = DateTime.Today;
                await _materialService.AddMaterialAsync(material);
                return RedirectToAction(nameof(Index));
            }

            return View(material);
        }
    }
}
