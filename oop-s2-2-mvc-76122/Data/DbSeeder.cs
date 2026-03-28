using System;
using System.Collections.Generic;
using System.Linq;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Premises.Any())
                return;

            // ----------- PREMISES (12) -----------
            var premisesList = new List<Premises>
            {
                new Premises { Name = "Cafe One", Address = "Street 1", Town = "Dublin", RiskRating = "Low" },
                new Premises { Name = "Burger Place", Address = "Street 2", Town = "Dublin", RiskRating = "High" },
                new Premises { Name = "Pizza Spot", Address = "Street 3", Town = "Cork", RiskRating = "Medium" },
                new Premises { Name = "Sushi Bar", Address = "Street 4", Town = "Galway", RiskRating = "High" },
                new Premises { Name = "Bakery Bliss", Address = "Street 5", Town = "Dublin", RiskRating = "Low" },
                new Premises { Name = "Taco Town", Address = "Street 6", Town = "Cork", RiskRating = "Medium" },
                new Premises { Name = "Steak House", Address = "Street 7", Town = "Galway", RiskRating = "High" },
                new Premises { Name = "Vegan Delight", Address = "Street 8", Town = "Dublin", RiskRating = "Low" },
                new Premises { Name = "Seafood Shack", Address = "Street 9", Town = "Cork", RiskRating = "High" },
                new Premises { Name = "Pasta Corner", Address = "Street 10", Town = "Galway", RiskRating = "Medium" },
                new Premises { Name = "BBQ Joint", Address = "Street 11", Town = "Dublin", RiskRating = "High" },
                new Premises { Name = "Healthy Bites", Address = "Street 12", Town = "Cork", RiskRating = "Low" }
            };

            context.Premises.AddRange(premisesList);
            context.SaveChanges();

            // ----------- INSPECTIONS (25) -----------
            var inspections = new List<Inspection>();
            var random = new Random();

            for (int i = 0; i < 25; i++)
            {
                var score = random.Next(30, 100);
                inspections.Add(new Inspection
                {
                    PremisesId = premisesList[random.Next(premisesList.Count)].Id,
                    InspectionDate = DateTime.Now.AddDays(-random.Next(1, 60)),
                    Score = score,
                    Outcome = score >= 50 ? "Pass" : "Fail",
                    Notes = "Routine inspection"
                });
            }

            context.Inspections.AddRange(inspections);
            context.SaveChanges();

            // ----------- FOLLOWUPS (10) -----------
            var followUps = new List<FollowUp>();

            for (int i = 0; i < 10; i++)
            {
                var isClosed = i % 2 == 0;

                followUps.Add(new FollowUp
                {
                    InspectionId = inspections[random.Next(inspections.Count)].Id,
                    DueDate = DateTime.Now.AddDays(random.Next(-10, 10)),
                    Status = isClosed ? "Closed" : "Open",
                    ClosedDate = isClosed ? DateTime.Now.AddDays(-1) : null
                });
            }

            context.FollowUps.AddRange(followUps);
            context.SaveChanges();
        }
    }
}