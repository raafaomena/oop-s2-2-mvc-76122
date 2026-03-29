using Microsoft.AspNetCore.Mvc;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;
using Microsoft.EntityFrameworkCore;

namespace oop_s2_2_mvc_76122.Controllers
{
    public class InspectionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InspectionController> _logger;

        public InspectionController(ApplicationDbContext context, ILogger<InspectionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Viewing inspections list");

            var inspections = await _context.Inspections.ToListAsync();
            return View(inspections);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Opening create inspection page");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Inspection inspection)
        {
            try
            {
                _logger.LogInformation("Creating inspection for Premises {PremisesId}", inspection.PremisesId);

                if (inspection.Score < 0 || inspection.Score > 100)
                {
                    _logger.LogWarning("Invalid inspection score: {Score}", inspection.Score);
                }

                _context.Add(inspection);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inspection");
                return View(inspection);
            }
        }
    }
}