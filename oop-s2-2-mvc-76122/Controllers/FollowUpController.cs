using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Controllers
{
    [Authorize]
    public class FollowUpController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FollowUpController> _logger;

        public FollowUpController(ApplicationDbContext context, ILogger<FollowUpController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: FollowUp
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var followUps = await _context.FollowUps
                .Include(f => f.Inspection)
                .ThenInclude(i => i.Premises)
                .ToListAsync();
            return View(followUps);
        }

        // GET: FollowUp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var followUp = await _context.FollowUps
                .Include(f => f.Inspection)
                .ThenInclude(i => i.Premises)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (followUp == null)
            {
                return NotFound();
            }

            return View(followUp);
        }

        // GET: FollowUp/Create
        [Authorize(Roles = "Admin,Inspector")]
        public IActionResult Create()
        {
            ViewBag.InspectionsList = _context.Inspections.Include(i => i.Premises).ToList();
            return View();
        }

        // POST: FollowUp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Create(FollowUp followUp)
        {
            if (ModelState.IsValid)
            {
                var inspection = await _context.Inspections.FindAsync(followUp.InspectionId);
                if (inspection != null && followUp.DueDate < inspection.InspectionDate)
                {
                    _logger.LogWarning("FollowUp due date before inspection date - FollowUpId: {Id}", followUp.Id);
                    ModelState.AddModelError("DueDate", "Due date cannot be before inspection date");
                    ViewBag.InspectionsList = _context.Inspections.Include(i => i.Premises).ToList();
                    return View(followUp);
                }

                _context.Add(followUp);
                await _context.SaveChangesAsync();
                _logger.LogInformation("FollowUp created by {User} - ID: {Id}", User.Identity?.Name, followUp.Id);
                TempData["Success"] = "Follow-up created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.InspectionsList = _context.Inspections.Include(i => i.Premises).ToList();
            return View(followUp);
        }

        // GET: FollowUp/Edit/5
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var followUp = await _context.FollowUps.FindAsync(id);
            if (followUp == null)
            {
                return NotFound();
            }
            ViewBag.InspectionsList = _context.Inspections.Include(i => i.Premises).ToList();
            return View(followUp);
        }

        // POST: FollowUp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int id, FollowUp followUp)
        {
            if (id != followUp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(followUp);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("FollowUp updated by {User} - ID: {Id}", User.Identity?.Name, followUp.Id);
                    TempData["Success"] = "Follow-up updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowUpExists(followUp.Id))
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
            ViewBag.InspectionsList = _context.Inspections.Include(i => i.Premises).ToList();
            return View(followUp);
        }

        // GET: FollowUp/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var followUp = await _context.FollowUps
                .Include(f => f.Inspection)
                .ThenInclude(i => i.Premises)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (followUp == null)
            {
                return NotFound();
            }

            return View(followUp);
        }

        // POST: FollowUp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var followUp = await _context.FollowUps.FindAsync(id);
            if (followUp != null)
            {
                _context.FollowUps.Remove(followUp);
                await _context.SaveChangesAsync();
                _logger.LogWarning("FollowUp deleted by {User} - ID: {Id}", User.Identity?.Name, id);
                TempData["Success"] = "Follow-up deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FollowUpExists(int id)
        {
            return _context.FollowUps.Any(e => e.Id == id);
        }
    }
}
