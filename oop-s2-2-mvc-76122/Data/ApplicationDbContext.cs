using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Premises> Premises { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<FollowUp> FollowUps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Premises>()
                .HasMany(p => p.Inspections)
                .WithOne(i => i.Premises)
                .HasForeignKey(i => i.PremisesId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inspection>()
                .HasMany(i => i.FollowUps)
                .WithOne(f => f.Inspection)
                .HasForeignKey(f => f.InspectionId)
                .OnDelete(DeleteBehavior.Cascade);

            var towns = new[] { "Bournemouth", "Poole", "Christchurch" };
            var riskRatings = new[] { RiskRating.Low, RiskRating.Medium, RiskRating.High };
            
            var premises = new List<Premises>();
            for (int i = 1; i <= 12; i++)
            {
                premises.Add(new Premises
                {
                    Id = i,
                    Name = $"Restaurant {i}",
                    Address = $"{i} High Street",
                    Town = towns[(i - 1) % 3],
                    RiskRating = riskRatings[(i - 1) % 3]
                });
            }
            modelBuilder.Entity<Premises>().HasData(premises);

            var random = new Random();
            var inspections = new List<Inspection>();
            var startDate = DateTime.Now.AddDays(-90);
            
            for (int i = 1; i <= 25; i++)
            {
                var inspectionDate = startDate.AddDays(random.Next(0, 90));
                var score = random.Next(0, 101);
                
                inspections.Add(new Inspection
                {
                    Id = i,
                    PremisesId = random.Next(1, 13),
                    InspectionDate = inspectionDate,
                    Score = score,
                    Outcome = score >= 60 ? Outcome.Pass : Outcome.Fail,
                    Notes = $"Inspection {i} - {(score >= 60 ? "Passed" : "Failed")} with score {score}"
                });
            }
            modelBuilder.Entity<Inspection>().HasData(inspections);

            var followUps = new List<FollowUp>();
            for (int i = 1; i <= 10; i++)
            {
                var inspection = inspections[(i - 1) % 25];
                var isClosed = i % 3 == 0;
                var dueDate = DateTime.Now.AddDays(isClosed ? -15 : random.Next(5, 30));
                
                followUps.Add(new FollowUp
                {
                    Id = i,
                    InspectionId = inspection.Id,
                    DueDate = dueDate,
                    Status = isClosed ? Status.Closed : Status.Open,
                    ClosedDate = isClosed ? DateTime.Now.AddDays(-5) : null
                });
            }
            modelBuilder.Entity<FollowUp>().HasData(followUps);
        }
    }
}