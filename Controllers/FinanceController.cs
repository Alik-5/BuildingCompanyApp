using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingCompanyApp.Controllers
{
    public class FinanceController : Controller
    {
        private readonly IFinanceService _financeService;
        private readonly IUserContextService _userContextService;

        public FinanceController(
            IFinanceService financeService,
            IUserContextService userContextService)
        {
            _financeService = financeService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var canManage = _userContextService.CanManageCompanyData();
            var entries = await _financeService.GetAllEntriesAsync();
            var invoices = canManage ? await _financeService.GetAllInvoicesAsync() : new List<Invoice>();
            var currentWorker = await _userContextService.GetCurrentWorkerAsync();

            if (!canManage && currentWorker != null)
            {
                entries = entries.Where(entry => entry.WorkerId == currentWorker.WorkerId).OrderByDescending(entry => entry.EntryDate).ToList();
            }

            return View(new FinancePageViewModel
            {
                CanManage = canManage,
                EmployeeName = currentWorker?.FullName ?? string.Empty,
                Salary = currentWorker?.Salary ?? 0,
                SalaryPaidThisYear = currentWorker == null
                    ? 0
                    : entries.Where(entry => entry.EntryDate.Year == DateTime.Today.Year).Sum(entry => entry.Amount),
                Entries = entries,
                Invoices = invoices
            });
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
        public async Task<IActionResult> Create(FinanceEntry entry)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                entry.EntryDate = entry.EntryDate == default ? DateTime.Today : entry.EntryDate;
                await _financeService.AddEntryAsync(entry);
                return RedirectToAction(nameof(Index));
            }

            return View(entry);
        }
    }
}
