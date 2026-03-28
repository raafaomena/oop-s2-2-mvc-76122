using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Premises> Premises { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<FollowUp> FollowUps { get; set; }
    }
}