using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;

namespace BuildingCompanyApp.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IWorkerService _workerService;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService _userContextService;

        public WorkerController(
            IWorkerService workerService,
            IUserRepository userRepository,
            IUserContextService userContextService)
        {
            _workerService = workerService;
            _userRepository = userRepository;
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

            var workers = await _workerService.GetAllWorkersAsync();
            return View(workers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var worker = await _workerService.GetWorkerByIdAsync(id);
            if (worker == null)
                return NotFound();

            return View(worker);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new WorkerUpsertViewModel
            {
                HireDate = DateTime.Today,
                WorkHours = 8,
                AccessRole = "Employee"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkerUpsertViewModel model)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.Email))
                {
                    ViewBag.Error = "Username, email and password are required for the employee login account.";
                    return View(model);
                }

                var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
                if (existingUser != null)
                {
                    ViewBag.Error = "This username already exists in Qore.";
                    return View(model);
                }

                var names = SplitName(model.FullName);
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    FirstName = names.firstName,
                    LastName = names.lastName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = model.AccessRole,
                    IsActive = true
                };
                await _userRepository.AddUserAsync(user);

                var worker = MapWorker(model);
                worker.UserId = user.UserId;
                worker.HireDate = worker.HireDate == default ? DateTime.Today : worker.HireDate;
                worker.AttendanceStatus = string.IsNullOrWhiteSpace(worker.AttendanceStatus) ? "Present" : worker.AttendanceStatus;
                await _workerService.AddWorkerAsync(worker);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var worker = await _workerService.GetWorkerByIdAsync(id);
            if (worker == null)
                return NotFound();

            var user = worker.UserId > 0 ? await _userRepository.GetUserByIdAsync(worker.UserId) : null;
            return View(new WorkerUpsertViewModel
            {
                WorkerId = worker.WorkerId,
                FullName = worker.FullName,
                Position = worker.Position,
                Role = worker.Role,
                Phone = worker.Phone,
                Address = worker.Address,
                Specialization = worker.Specialization,
                HireDate = worker.HireDate,
                Salary = worker.Salary,
                AttendanceStatus = worker.AttendanceStatus,
                WorkHours = worker.WorkHours,
                ProfileImageUrl = worker.ProfileImageUrl,
                Username = user?.Username ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                AccessRole = user?.Role ?? "Employee",
                HasExistingAccount = user != null
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, WorkerUpsertViewModel model)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (id != model.WorkerId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var existingWorker = await _workerService.GetWorkerByIdAsync(id);
                if (existingWorker == null)
                {
                    return NotFound();
                }

                var existingUser = existingWorker.UserId > 0 ? await _userRepository.GetUserByIdAsync(existingWorker.UserId) : null;
                var duplicateUser = await _userRepository.GetUserByUsernameAsync(model.Username);
                if (duplicateUser != null && duplicateUser.UserId != existingUser?.UserId)
                {
                    ViewBag.Error = "This username already exists in Qore.";
                    return View(model);
                }

                if (existingUser != null)
                {
                    var names = SplitName(model.FullName);
                    existingUser.Username = model.Username;
                    existingUser.Email = model.Email;
                    existingUser.FirstName = names.firstName;
                    existingUser.LastName = names.lastName;
                    existingUser.Role = model.AccessRole;
                    if (!string.IsNullOrWhiteSpace(model.Password))
                    {
                        existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    }
                    await _userRepository.UpdateUserAsync(existingUser);
                }

                var worker = MapWorker(model);
                worker.UserId = existingWorker.UserId;
                worker.CreatedDate = existingWorker.CreatedDate;
                await _workerService.UpdateWorkerAsync(worker);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_userContextService.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!_userContextService.CanManageCompanyData())
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var worker = await _workerService.GetWorkerByIdAsync(id);
            if (worker?.UserId > 0)
            {
                await _userRepository.DeleteUserAsync(worker.UserId);
            }

            await _workerService.DeleteWorkerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private static Worker MapWorker(WorkerUpsertViewModel model)
        {
            return new Worker
            {
                WorkerId = model.WorkerId,
                FullName = model.FullName,
                Position = model.Position,
                Role = model.Role,
                Phone = model.Phone,
                Address = model.Address,
                Specialization = model.Specialization,
                HireDate = model.HireDate,
                Salary = model.Salary,
                AttendanceStatus = model.AttendanceStatus,
                WorkHours = model.WorkHours,
                ProfileImageUrl = model.ProfileImageUrl
            };
        }

        private static (string firstName, string lastName) SplitName(string fullName)
        {
            var parts = (fullName ?? string.Empty)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
            {
                return ("Qore", "User");
            }

            return parts.Length == 1
                ? (parts[0], "Team")
                : (parts[0], string.Join(' ', parts.Skip(1)));
        }
    }
}
