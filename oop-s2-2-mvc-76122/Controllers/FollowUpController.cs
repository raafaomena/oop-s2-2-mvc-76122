using Microsoft.AspNetCore.Mvc;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;
using Serilog;

namespace oop_s2_2_mvc_76122.Controllers
{
    public class FollowUpController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FollowUpController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var followUps = _context.FollowUps.ToList();
            return View(followUps);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(FollowUp followUp)
        {
            try
            {
                var inspection = _context.Inspections.Find(followUp.InspectionId);

                if (inspection != null && followUp.DueDate < inspection.InspectionDate)
                {
                    Log.Warning("FollowUp due date before inspection date");
                    return View(followUp);
                }

                _context.FollowUps.Add(followUp);
                _context.SaveChanges();

                Log.Information("FollowUp created for InspectionId {InspectionId}", followUp.InspectionId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating follow-up");
                return View("Error");
            }
        }
    }
}