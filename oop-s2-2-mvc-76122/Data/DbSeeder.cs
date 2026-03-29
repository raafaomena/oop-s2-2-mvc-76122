using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Premises.Any()) return;

        var premises = new List<Premises>
        {
            new Premises { Name="Cafe A", Address="Street 1", Town="Dublin", RiskRating="Low" },
            new Premises { Name="Cafe B", Address="Street 2", Town="Dublin", RiskRating="High" },
            new Premises { Name="Cafe C", Address="Street 3", Town="Cork", RiskRating="Medium" },
            new Premises { Name="Cafe D", Address="Street 4", Town="Cork", RiskRating="Low" },
            new Premises { Name="Cafe E", Address="Street 5", Town="Galway", RiskRating="High" },
            new Premises { Name="Cafe F", Address="Street 6", Town="Galway", RiskRating="Medium" },
            new Premises { Name="Cafe G", Address="Street 7", Town="Dublin", RiskRating="Low" },
            new Premises { Name="Cafe H", Address="Street 8", Town="Dublin", RiskRating="High" },
            new Premises { Name="Cafe I", Address="Street 9", Town="Cork", RiskRating="Medium" },
            new Premises { Name="Cafe J", Address="Street 10", Town="Galway", RiskRating="Low" },
            new Premises { Name="Cafe K", Address="Street 11", Town="Cork", RiskRating="High" },
            new Premises { Name="Cafe L", Address="Street 12", Town="Dublin", RiskRating="Medium" }
        };

        context.Premises.AddRange(premises);
        context.SaveChanges();

        var inspections = new List<Inspection>();

        for (int i = 0; i < 25; i++)
        {
            inspections.Add(new Inspection
            {
                PremisesId = premises[i % 12].Id,
                InspectionDate = DateTime.Now.AddDays(-i),
                Score = Random.Shared.Next(50, 100),
                Outcome = i % 2 == 0 ? "Pass" : "Fail",
                Notes = "Routine check"
            });
        }

        context.Inspections.AddRange(inspections);
        context.SaveChanges();

        var followUps = new List<FollowUp>();

        for (int i = 0; i < 10; i++)
        {
            followUps.Add(new FollowUp
            {
                InspectionId = inspections[i].Id,
                DueDate = DateTime.Now.AddDays(i - 5),
                Status = i % 2 == 0 ? "Open" : "Closed",
                ClosedDate = i % 2 == 0 ? null : DateTime.Now
            });
        }

        context.FollowUps.AddRange(followUps);
        context.SaveChanges();
    }
}