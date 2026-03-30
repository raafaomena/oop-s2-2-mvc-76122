using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Data;
using oop_s2_2_mvc_76122.Models;
using Xunit;

namespace oop_s2_2_mvc_76122.Tests
{
    public class DashboardTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            var context = new ApplicationDbContext(options);
            SeedTestData(context);
            return context;
        }

        private void SeedTestData(ApplicationDbContext context)
        {
            var premises = new Premises
            {
                Id = 1,
                Name = "Test Restaurant",
                Address = "123 Test St",
                Town = "Bournemouth",
                RiskRating = RiskRating.Medium
            };
            context.Premises.Add(premises);

            var today = DateTime.Today;
            var inspection = new Inspection
            {
                Id = 1,
                PremisesId = 1,
                InspectionDate = today,
                Score = 45,
                Outcome = Outcome.Fail,
                Notes = "Test inspection"
            };
            context.Inspections.Add(inspection);

            var followUp = new FollowUp
            {
                Id = 1,
                InspectionId = 1,
                DueDate = today.AddDays(-5),
                Status = Status.Open,
                ClosedDate = null
            };
            context.FollowUps.Add(followUp);

            context.SaveChanges();
        }

        [Fact]
        public void OverdueFollowUps_ReturnsCorrectItems()
        {
            var context = GetDbContext();
            var today = DateTime.Today;
            var overdueCount = context.FollowUps.Count(f => f.Status == Status.Open && f.DueDate < today);
            Assert.Equal(1, overdueCount);
        }

        [Fact]
        public void DashboardCounts_ConsistentWithSeedData()
        {
            var context = GetDbContext();
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var inspectionsThisMonth = context.Inspections.Count(i => i.InspectionDate >= firstDayOfMonth);
            var failedInspections = context.Inspections.Count(i => i.InspectionDate >= firstDayOfMonth && i.Outcome == Outcome.Fail);
            var overdueFollowUps = context.FollowUps.Count(f => f.Status == Status.Open && f.DueDate < today);
            Assert.Equal(1, inspectionsThisMonth);
            Assert.Equal(1, failedInspections);
            Assert.Equal(1, overdueFollowUps);
        }

        [Fact]
        public void FollowUp_CanBeCreatedWithValidData()
        {
            var context = GetDbContext();
            var followUp = new FollowUp
            {
                InspectionId = 1,
                DueDate = DateTime.Today.AddDays(10),
                Status = Status.Open,
                ClosedDate = null
            };
            context.FollowUps.Add(followUp);
            context.SaveChanges();
            Assert.Equal(2, context.FollowUps.Count());
        }

        [Fact]
        public void Premises_HasCorrectRiskRating()
        {
            var context = GetDbContext();
            var premises = context.Premises.FirstOrDefault(p => p.Id == 1);
            Assert.NotNull(premises);
            Assert.Equal(RiskRating.Medium, premises.RiskRating);
        }
    }
}
