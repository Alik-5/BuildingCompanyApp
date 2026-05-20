using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;

namespace BuildingCompanyApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IUserContextService _userContextService;

        public DashboardController(IDashboardService dashboardService, IUserContextService userContextService)
        {
            _dashboardService = dashboardService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var metrics = await _dashboardService.GetDashboardMetricsAsync();
            return View(metrics);
        }
    }
}
