using Microsoft.AspNetCore.Mvc;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home page accessed");
            return View();
        }

        public IActionResult Error(string message)
        {
            _logger.LogError("Error page triggered: {Message}", message);
            var errorViewModel = new ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = message
            };
            return View(errorViewModel);
        }
    }
}
