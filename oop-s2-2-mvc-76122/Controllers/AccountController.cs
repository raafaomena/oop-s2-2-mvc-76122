using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace oop_s2_2_mvc_76122.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    // 🔹 LOGIN NORMAL
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

        if (result.Succeeded)
            return RedirectToAction("Index", "Dashboard");

        ViewBag.Error = "Invalid login";
        return View();
    }

    // 🔹 QUICK LOGIN (ESSENCIAL PRA TESTAR ROLES)
    public async Task<IActionResult> QuickLogin(string role)
    {
        string email = role switch
        {
            "Admin" => "admin@test.com",
            "Inspector" => "inspector@test.com",
            "Viewer" => "viewer@test.com",
            _ => "admin@test.com"
        };

        await _signInManager.SignOutAsync();

        await _signInManager.PasswordSignInAsync(email, role + "123!", false, false);

        return RedirectToAction("Index", "Dashboard");
    }

    // 🔹 LOGOUT
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}