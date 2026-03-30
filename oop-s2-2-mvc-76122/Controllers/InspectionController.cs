using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Controllers;

public class InspectionController : Controller
{
    private readonly ApplicationDbContext _context;

    public InspectionController(ApplicationDbContext context)
    {
        _context = context;
    }

    // LIST
    public async Task<IActionResult> Index()
    {
        var inspections = await _context.Inspections
            .Include(i => i.Premises)
            .ToListAsync();

        return View(inspections);
    }

    // CREATE
    public IActionResult Create()
    {
        ViewBag.Premises = _context.Premises.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Inspection inspection)
    {
        _context.Add(inspection);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT
    public async Task<IActionResult> Edit(int id)
    {
        var inspection = await _context.Inspections.FindAsync(id);
        ViewBag.Premises = _context.Premises.ToList();

        return View(inspection);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Inspection inspection)
    {
        _context.Update(inspection);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var inspection = await _context.Inspections.FindAsync(id);

        if (inspection != null)
        {
            _context.Inspections.Remove(inspection);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}