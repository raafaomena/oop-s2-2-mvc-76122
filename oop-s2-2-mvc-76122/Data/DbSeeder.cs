using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Evita duplicar dados
            if (context.Premises.Any()) return;

            // -------------------------
            // PREMISES
            // -------------------------
            var premises = new List<Premises>
            {
                new Premises { Name = "Cafe One", Address = "Street 1", Town = "Dublin", RiskRating = "Low" },
                new Premises { Name = "Burger Place", Address = "Street 2", Town = "Dublin", RiskRating = "High" },
                new Premises { Name = "Pizza Spot", Address = "Street 3", Town = "Cork", RiskRating = "Medium" },
                new Premises { Name = "Sushi Bar", Address = "Street 4", Town = "Galway", RiskRating = "High" }
            };

            context.Premises.AddRange(premises);
            context.SaveChanges();

            // -------------------------
            // INSPECTIONS (AGORA COM NOTES)
            // -------------------------
            var inspections = new List<Inspection>
            {
                new Inspection 
                { 
                    PremisesId = 1, 
                    InspectionDate = DateTime.Now.AddDays(-10), 
                    Score = 85, 
                    Outcome = "Pass",
                    Notes = "Good hygiene standards"
                },
                new Inspection 
                { 
                    PremisesId = 2, 
                    InspectionDate = DateTime.Now.AddDays(-5), 
                    Score = 40, 
                    Outcome = "Fail",
                    Notes = "Poor food storage"
                },
                new Inspection 
                { 
                    PremisesId = 3, 
                    InspectionDate = DateTime.Now.AddDays(-20), 
                    Score = 70, 
                    Outcome = "Pass",
                    Notes = "Minor issues found"
                }
            };

            context.Inspections.AddRange(inspections);
            context.SaveChanges();

            // -------------------------
            // FOLLOWUPS
            // -------------------------
            var followUps = new List<FollowUp>
            {
                new FollowUp 
                { 
                    InspectionId = 2, 
                    DueDate = DateTime.Now.AddDays(-2), 
                    Status = "Open"
                },
                new FollowUp 
                { 
                    InspectionId = 2, 
                    DueDate = DateTime.Now.AddDays(5), 
                    Status = "Open"
                },
                new FollowUp 
                { 
                    InspectionId = 1, 
                    DueDate = DateTime.Now.AddDays(-1), 
                    Status = "Closed",
                    ClosedDate = DateTime.Now
                }
            };

            context.FollowUps.AddRange(followUps);
            context.SaveChanges();
        }
    }
}