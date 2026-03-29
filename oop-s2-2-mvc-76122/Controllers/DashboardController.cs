using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;

namespace oop_s2_2_mvc_76122.Controllers;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string town, string risk)
    {
        var inspections = _context.Inspections.Include(i => i.Premises).AsQueryable();

        if (!string.IsNullOrEmpty(town))
            inspections = inspections.Where(i => i.Premises.Town == town);

        if (!string.IsNullOrEmpty(risk))
            inspections = inspections.Where(i => i.Premises.RiskRating == risk);

        var overdue = _context.FollowUps
            .Count(f => f.Status == "Open" && f.DueDate < DateTime.Now);

        ViewBag.Overdue = overdue;

        return View(inspections.ToList());
    }
}