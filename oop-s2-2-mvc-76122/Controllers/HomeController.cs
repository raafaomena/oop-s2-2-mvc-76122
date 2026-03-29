using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Error()
        {
            _logger.LogError("Error page triggered");
            return View();
        }
    }
}