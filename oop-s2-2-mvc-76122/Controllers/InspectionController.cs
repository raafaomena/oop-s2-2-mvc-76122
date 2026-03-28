using Microsoft.AspNetCore.Mvc;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;
using Serilog;

public class InspectionController : Controller
{
    private readonly ApplicationDbContext _context;

    public InspectionController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Inspection inspection)
    {
        try
        {
            if (inspection.Score < 0 || inspection.Score > 100)
            {
                Log.Warning("Invalid score {Score} for inspection", inspection.Score);
                return View(inspection);
            }

            _context.Inspections.Add(inspection);
            _context.SaveChanges();

            Log.Information("Inspection created with PremisesId {PremisesId} and Score {Score}",
                inspection.PremisesId, inspection.Score);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating inspection");
            return View("Error");
        }
    }
}