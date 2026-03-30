using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Premises.Any()) return;

        var premises = new List<Premises>();

        string[] towns = { "Dublin", "Cork", "Galway" };
        string[] risks = { "Low", "Medium", "High" };

        for (int i = 1; i <= 12; i++)
        {
            premises.Add(new Premises
            {
                Name = $"Restaurant {i}",
                Address = $"Street {i}",
                Town = towns[i % 3],
                RiskRating = risks[i % 3]
            });
        }

        context.Premises.AddRange(premises);
        context.SaveChanges();

        var inspections = new List<Inspection>();

        for (int i = 1; i <= 25; i++)
        {
            inspections.Add(new Inspection
            {
                PremisesId = premises[i % 12].Id,
                InspectionDate = DateTime.Now.AddDays(-i * 2),
                Score = Random.Shared.Next(50, 100),
                Outcome = i % 2 == 0 ? "Pass" : "Fail",
                Notes = "Routine inspection"
            });
        }

        context.Inspections.AddRange(inspections);
        context.SaveChanges();

        var followUps = new List<FollowUp>();

        for (int i = 1; i <= 10; i++)
        {
            followUps.Add(new FollowUp
            {
                InspectionId = inspections[i % 25].Id,
                DueDate = DateTime.Now.AddDays(i % 2 == 0 ? -3 : 3),
                Status = i % 2 == 0 ? "Open" : "Closed",
                ClosedDate = i % 2 == 0 ? null : DateTime.Now
            });
        }

        context.FollowUps.AddRange(followUps);
        context.SaveChanges();
    }
}