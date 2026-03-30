using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and password are required";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in: {Email}", email);
                return RedirectToAction("Index", "Dashboard");
            }
            
            ViewBag.Error = "Invalid login attempt";
            return View();
        }

        public async Task<IActionResult> QuickLogin(string role)
        {
            var email = $"{role?.ToLower()}@foodsafety.com";
            var password = $"{role}@123";
            
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Email}", email);
                return RedirectToAction("Login");
            }

            await _signInManager.SignOutAsync();
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("Quick login as {Role}: {Email}", role, email);
                return RedirectToAction("Index", "Dashboard");
            }
            
            _logger.LogWarning("Quick login failed for {Email}", email);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");
            return RedirectToAction("Login");
        }
    }
}
