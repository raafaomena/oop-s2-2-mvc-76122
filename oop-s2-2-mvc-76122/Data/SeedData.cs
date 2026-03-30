using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            if (!context.Premises.Any())
            {
                var towns = new[] { "Bournemouth", "Poole", "Christchurch" };
                var riskRatings = new[] { RiskRating.Low, RiskRating.Medium, RiskRating.High };
                
                var premises = new List<Premises>();
                for (int i = 1; i <= 12; i++)
                {
                    premises.Add(new Premises
                    {
                        Name = $"Restaurant {i}",
                        Address = $"{i} High Street",
                        Town = towns[(i - 1) % 3],
                        RiskRating = riskRatings[(i - 1) % 3]
                    });
                }
                context.Premises.AddRange(premises);
                await context.SaveChangesAsync();
            }

            if (!context.Inspections.Any())
            {
                var random = new Random();
                var premises = await context.Premises.ToListAsync();
                var startDate = DateTime.Now.AddDays(-90);
                var inspections = new List<Inspection>();
                
                for (int i = 1; i <= 25; i++)
                {
                    var inspectionDate = startDate.AddDays(random.Next(0, 90));
                    var score = random.Next(0, 101);
                    
                    inspections.Add(new Inspection
                    {
                        PremisesId = premises[random.Next(premises.Count)].Id,
                        InspectionDate = inspectionDate,
                        Score = score,
                        Outcome = score >= 60 ? Outcome.Pass : Outcome.Fail,
                        Notes = $"Inspection {i} - {(score >= 60 ? "Passed" : "Failed")} with score {score}"
                    });
                }
                context.Inspections.AddRange(inspections);
                await context.SaveChangesAsync();
            }

            if (!context.FollowUps.Any())
            {
                var random = new Random();
                var inspections = await context.Inspections.ToListAsync();
                var followUps = new List<FollowUp>();
                
                for (int i = 1; i <= 10; i++)
                {
                    var inspection = inspections[(i - 1) % inspections.Count];
                    var isClosed = i % 3 == 0;
                    var dueDate = DateTime.Now.AddDays(isClosed ? -15 : random.Next(5, 30));
                    
                    followUps.Add(new FollowUp
                    {
                        InspectionId = inspection.Id,
                        DueDate = dueDate,
                        Status = isClosed ? Status.Closed : Status.Open,
                        ClosedDate = isClosed ? DateTime.Now.AddDays(-5) : null
                    });
                }
                context.FollowUps.AddRange(followUps);
                await context.SaveChangesAsync();
            }

            string[] roleNames = { "Admin", "Inspector", "Viewer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "admin@foodsafety.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var inspectorEmail = "inspector@foodsafety.com";
            if (await userManager.FindByEmailAsync(inspectorEmail) == null)
            {
                var inspectorUser = new IdentityUser
                {
                    UserName = inspectorEmail,
                    Email = inspectorEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(inspectorUser, "Inspector@123");
                await userManager.AddToRoleAsync(inspectorUser, "Inspector");
            }

            var viewerEmail = "viewer@foodsafety.com";
            if (await userManager.FindByEmailAsync(viewerEmail) == null)
            {
                var viewerUser = new IdentityUser
                {
                    UserName = viewerEmail,
                    Email = viewerEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(viewerUser, "Viewer@123");
                await userManager.AddToRoleAsync(viewerUser, "Viewer");
            }
        }
    }
}
