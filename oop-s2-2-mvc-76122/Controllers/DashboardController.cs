using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string town, string risk)
        {
            var thisMonth = DateTime.Now.Month;

            var inspections = _context.Inspections
                .Include(i => i.Premises)
                .AsQueryable();

            // filtros
            if (!string.IsNullOrEmpty(town))
            {
                inspections = inspections.Where(i => i.Premises.Town == town);
            }

            if (!string.IsNullOrEmpty(risk))
            {
                inspections = inspections.Where(i => i.Premises.RiskRating == risk);
            }

            var model = new DashboardViewModel
            {
                InspectionsThisMonth = inspections.Count(i => i.InspectionDate.Month == thisMonth),
                FailedInspections = inspections.Count(i => i.InspectionDate.Month == thisMonth && i.Outcome == "Fail"),
                OverdueFollowUps = _context.FollowUps
                    .Count(f => f.DueDate < DateTime.Now && f.Status == "Open"),

                SelectedTown = town,
                SelectedRisk = risk
            };

            return View(model);
        }
    }
}