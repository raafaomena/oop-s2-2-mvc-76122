using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Controllers
{
    [Authorize]
    public class InspectionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InspectionController> _logger;

        public InspectionController(ApplicationDbContext context, ILogger<InspectionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Inspection
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var inspections = await _context.Inspections
                .Include(i => i.Premises)
                .Include(i => i.FollowUps)
                .ToListAsync();
            return View(inspections);
        }

        // GET: Inspection/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspections
                .Include(i => i.Premises)
                .Include(i => i.FollowUps)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (inspection == null)
            {
                return NotFound();
            }

            return View(inspection);
        }

        // GET: Inspection/Create
        [Authorize(Roles = "Admin,Inspector")]
        public IActionResult Create()
        {
            ViewBag.PremisesList = _context.Premises.ToList();
            return View();
        }

        // POST: Inspection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Create(Inspection inspection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inspection);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Inspection created by {User} - ID: {Id}", User.Identity?.Name, inspection.Id);
                TempData["Success"] = "Inspection created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.PremisesList = _context.Premises.ToList();
            return View(inspection);
        }

        // GET: Inspection/Edit/5
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }
            ViewBag.PremisesList = _context.Premises.ToList();
            return View(inspection);
        }

        // POST: Inspection/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int id, Inspection inspection)
        {
            if (id != inspection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inspection);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Inspection updated by {User} - ID: {Id}", User.Identity?.Name, inspection.Id);
                    TempData["Success"] = "Inspection updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InspectionExists(inspection.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.PremisesList = _context.Premises.ToList();
            return View(inspection);
        }

        // GET: Inspection/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspections
                .Include(i => i.Premises)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inspection == null)
            {
                return NotFound();
            }

            return View(inspection);
        }

        // POST: Inspection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection != null)
            {
                _context.Inspections.Remove(inspection);
                await _context.SaveChangesAsync();
                _logger.LogWarning("Inspection deleted by {User} - ID: {Id}", User.Identity?.Name, id);
                TempData["Success"] = "Inspection deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InspectionExists(int id)
        {
            return _context.Inspections.Any(e => e.Id == id);
        }
    }
}
