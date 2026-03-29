using Microsoft.AspNetCore.Mvc;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;
using Microsoft.EntityFrameworkCore;

namespace oop_s2_2_mvc_76122.Controllers
{
    public class FollowUpController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FollowUpController> _logger;

        public FollowUpController(ApplicationDbContext context, ILogger<FollowUpController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Viewing follow-ups list");

            var list = await _context.FollowUps.ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Opening create follow-up page");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FollowUp followUp)
        {
            try
            {
                _logger.LogInformation("Creating FollowUp for Inspection {InspectionId}", followUp.InspectionId);

                if (followUp.DueDate < DateTime.Now)
                {
                    _logger.LogWarning("FollowUp due date is in the past");
                }

                _context.Add(followUp);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating follow-up");
                return View(followUp);
            }
        }
    }
}