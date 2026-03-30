using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_76122.Models;

namespace oop_s2_2_mvc_76122.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Premises> Premises { get; set; }
    public DbSet<Inspection> Inspections { get; set; }
    public DbSet<FollowUp> FollowUps { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Inspection>()
            .HasOne(i => i.Premises)
            .WithMany(p => p.Inspections)
            .HasForeignKey(i => i.PremisesId);

        builder.Entity<FollowUp>()
            .HasOne(f => f.Inspection)
            .WithMany(i => i.FollowUps)
            .HasForeignKey(f => f.InspectionId);
    }
}