using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Premises.Any()) return;

            var premisesList = new List<Premises>();

            for (int i = 1; i <= 12; i++)
            {
                premisesList.Add(new Premises
                {
                    Name = $"Restaurant {i}",
                    Address = $"Street {i}",
                    Town = i % 3 == 0 ? "Dublin" : i % 3 == 1 ? "Cork" : "Galway",
                    RiskRating = i % 2 == 0 ? "High" : "Low"
                });
            }

            context.Premises.AddRange(premisesList);
            context.SaveChanges();

            var inspections = new List<Inspection>();

            for (int i = 1; i <= 25; i++)
            {
                inspections.Add(new Inspection
                {
                    PremisesId = premisesList[i % 12].Id,
                    InspectionDate = DateTime.Now.AddDays(-i),
                    Score = 50 + i,
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
                    InspectionId = inspections[i].Id,
                    DueDate = DateTime.Now.AddDays(i),
                    Status = i % 2 == 0 ? "Closed" : "Open",
                    ClosedDate = i % 2 == 0 ? DateTime.Now : null
                });
            }

            context.FollowUps.AddRange(followUps);
            context.SaveChanges();
        }
    }
}