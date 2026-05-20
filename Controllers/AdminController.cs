using Microsoft.AspNetCore.Mvc;
using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;

namespace BuildingCompanyApp.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AdminController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        // Check if user is Admin
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }

        private IActionResult? RequireAdmin()
        {
            if (!IsAdmin())
                return RedirectToAction("Unauthorized", "Home");
            return null;
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var check = RequireAdmin();
            if (check != null) return check;

            var users = await _userRepository.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet("CreateUser")]
        public IActionResult CreateUser()
        {
            var check = RequireAdmin();
            if (check != null) return check;

            return View(new CreateUserRequest());
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var check = RequireAdmin();
            if (check != null) return check;

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill all required fields correctly.";
                return View(request);
            }

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                ViewBag.Error = "Username is required.";
                return View(request);
            }

            // Check if username/email already exists
            var existingUser = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                ViewBag.Error = "Username already exists!";
                return View(request);
            }

            try
            {
                var user = new User
                {
                    UserId = new Random().Next(100, 10000),
                    Username = request.Username,
                    Email = request.Email ?? "no-email@qore.am",
                    FirstName = request.FirstName ?? "User",
                    LastName = request.LastName ?? "Qore",
                    Role = request.Role,  // "Admin", "Manager", or "Employee"
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password ?? "TempPassword123!"),
                    IsActive = true
                };

                await _userRepository.AddUserAsync(user);
                ViewBag.Success = $"✓ User '{request.Username}' created successfully with role '{request.Role}'";
                return View(new CreateUserRequest());
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error creating user: {ex.Message}";
                return View(request);
            }
        }

        [HttpGet("EditUser/{userId}")]
        public async Task<IActionResult> EditUser(int userId)
        {
            var check = RequireAdmin();
            if (check != null) return check;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost("EditUser/{userId}")]
        public async Task<IActionResult> EditUser(int userId, User user)
        {
            var check = RequireAdmin();
            if (check != null) return check;

            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
                return NotFound();

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.IsActive = user.IsActive;

            await _userRepository.UpdateUserAsync(existingUser);
            ViewBag.Success = "User updated successfully!";
            return View(existingUser);
        }

        [HttpPost("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var check = RequireAdmin();
            if (check != null) return check;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                await _userRepository.UpdateUserAsync(user);
            }

            return RedirectToAction("Dashboard");
        }
    }

    public class CreateUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; } = "Employee";  // Default role
    }
}
