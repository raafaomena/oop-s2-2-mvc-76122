using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (context.Premises.Any())
                return;

            var premises = new List<Premises>
            {
                new Premises { Name = "Cafe Dublin", Address = "1 Main St", Town = "Dublin", RiskRating = "Low" },
                new Premises { Name = "Burger Town", Address = "2 Main St", Town = "Dublin", RiskRating = "High" },
                new Premises { Name = "Pizza Place", Address = "3 Main St", Town = "Dublin", RiskRating = "Medium" },

                new Premises { Name = "Cork Bites", Address = "1 River Rd", Town = "Cork", RiskRating = "Low" },
                new Premises { Name = "Steak House", Address = "2 River Rd", Town = "Cork", RiskRating = "High" },
                new Premises { Name = "Sushi Cork", Address = "3 River Rd", Town = "Cork", RiskRating = "Medium" },

                new Premises { Name = "Galway Grill", Address = "1 Sea Rd", Town = "Galway", RiskRating = "Low" },
                new Premises { Name = "Fish Bar", Address = "2 Sea Rd", Town = "Galway", RiskRating = "High" },
                new Premises { Name = "Cafe West", Address = "3 Sea Rd", Town = "Galway", RiskRating = "Medium" },

                new Premises { Name = "Dublin Deli", Address = "4 Main St", Town = "Dublin", RiskRating = "Low" },
                new Premises { Name = "Cork Cafe", Address = "4 River Rd", Town = "Cork", RiskRating = "Medium" },
                new Premises { Name = "Galway Eats", Address = "4 Sea Rd", Town = "Galway", RiskRating = "High" }
            };

            context.Premises.AddRange(premises);
            context.SaveChanges();

            var inspections = new List<Inspection>();

            foreach (var p in premises)
            {
                for (int i = 0; i < 2; i++)
                {
                    inspections.Add(new Inspection
                    {
                        PremisesId = p.Id,
                        InspectionDate = DateTime.Now.AddDays(-i * 10),
                        Score = new Random().Next(50, 100),
                        Outcome = "Pass",
                        Notes = "Routine inspection"
                    });
                }
            }

            context.Inspections.AddRange(inspections);
            context.SaveChanges();

            var followUps = new List<FollowUp>
            {
                new FollowUp { InspectionId = inspections[0].Id, DueDate = DateTime.Now.AddDays(-5), Status = "Open" },
                new FollowUp { InspectionId = inspections[1].Id, DueDate = DateTime.Now.AddDays(5), Status = "Open" },
                new FollowUp { InspectionId = inspections[2].Id, DueDate = DateTime.Now.AddDays(-2), Status = "Closed", ClosedDate = DateTime.Now },
                new FollowUp { InspectionId = inspections[3].Id, DueDate = DateTime.Now.AddDays(3), Status = "Open" },
                new FollowUp { InspectionId = inspections[4].Id, DueDate = DateTime.Now.AddDays(-1), Status = "Open" },
                new FollowUp { InspectionId = inspections[5].Id, DueDate = DateTime.Now.AddDays(7), Status = "Closed", ClosedDate = DateTime.Now }
            };

            context.FollowUps.AddRange(followUps);
            context.SaveChanges();
        }
    }
}