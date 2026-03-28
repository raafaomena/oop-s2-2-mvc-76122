using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;
using Serilog;

namespace oop_s2_2_mvc_76122.Controllers
{
    public class InspectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InspectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var inspections = _context.Inspections
                .Include(i => i.Premises)
                .ToList();

            return View(inspections);
        }

        public IActionResult Create()
        {
            ViewBag.Premises = _context.Premises.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Inspection inspection)
        {
            try
            {
                if (inspection.Score < 0 || inspection.Score > 100)
                {
                    Log.Warning("Invalid score {Score}", inspection.Score);
                    return View(inspection);
                }

                _context.Inspections.Add(inspection);
                _context.SaveChanges();

                Log.Information("Inspection created for PremisesId {PremisesId}", inspection.PremisesId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating inspection");
                return View("Error");
            }
        }
    }
}