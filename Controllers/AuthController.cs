using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BuildingCompanyApp.Models;
using BuildingCompanyApp.Services;

namespace BuildingCompanyApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.AuthenticateAsync(request.Username, request.Password);
            if (result != null)
            {
                // Set authentication cookie or JWT token
                HttpContext.Session.SetString("UserId", result.UserId.ToString());
                HttpContext.Session.SetString("Username", result.Username);
                HttpContext.Session.SetString("Role", result.Role);
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Error = "Registration failed. Username or email may already exist.";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
