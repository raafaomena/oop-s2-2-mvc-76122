using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Controllers;

[Authorize(Roles = "Admin,Inspector")]
public class FollowUpController : Controller
{
    private readonly ApplicationDbContext _context;

    public FollowUpController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.FollowUps.Include(f => f.Inspection).ToListAsync());
    }

    public IActionResult Create()
    {
        ViewBag.Inspections = _context.Inspections.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FollowUp followUp)
    {
        if (ModelState.IsValid)
        {
            _context.Add(followUp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(followUp);
    }
}