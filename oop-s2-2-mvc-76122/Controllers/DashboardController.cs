using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;
using oop_s2_2_mvc_76122.Models.ViewModels;

namespace oop_s2_2_mvc_76122.Controllers
{
    [Authorize(Roles = "Admin,Viewer")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? town, RiskRating? riskRating)
        {
            try
            {
                var userRole = User.IsInRole("Admin") ? "Admin" : "Viewer";
                _logger.LogInformation("Dashboard accessed by {User} (Role: {Role})", User.Identity?.Name, userRole);

                var query = _context.Inspections
                    .Include(i => i.Premises)
                    .Include(i => i.FollowUps)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(town))
                {
                    query = query.Where(i => i.Premises != null && i.Premises.Town == town);
                    _logger.LogInformation("Filtering by town: {Town}", town);
                }
                    
                if (riskRating.HasValue)
                {
                    query = query.Where(i => i.Premises != null && i.Premises.RiskRating == riskRating.Value);
                    _logger.LogInformation("Filtering by risk rating: {RiskRating}", riskRating.Value);
                }

                var today = DateTime.Today;
                var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

                var inspections = await query.ToListAsync();
                var followUps = inspections.SelectMany(i => i.FollowUps ?? new List<FollowUp>()).ToList();

                var viewModel = new DashboardViewModel
                {
                    InspectionsThisMonth = inspections.Count(i => i.InspectionDate >= firstDayOfMonth),
                    FailedInspectionsThisMonth = inspections.Count(i => i.InspectionDate >= firstDayOfMonth && i.Outcome == Outcome.Fail),
                    OverdueFollowUps = followUps.Count(f => f.Status == Status.Open && f.DueDate < today),
                    TotalPremises = await _context.Premises.CountAsync(),
                    TotalInspections = await _context.Inspections.CountAsync(),
                    OpenFollowUps = await _context.FollowUps.CountAsync(f => f.Status == Status.Open),
                    Towns = await _context.Premises.Select(p => p.Town).Distinct().ToListAsync(),
                    SelectedTown = town,
                    SelectedRiskRating = riskRating
                };

                _logger.LogInformation(
                    "Dashboard summary - InspectionsThisMonth: {InspectionsThisMonth}, " +
                    "FailedInspectionsThisMonth: {FailedInspectionsThisMonth}, " +
                    "OverdueFollowUps: {OverdueFollowUps}",
                    viewModel.InspectionsThisMonth,
                    viewModel.FailedInspectionsThisMonth,
                    viewModel.OverdueFollowUps);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard");
                throw;
            }
        }
    }
}
