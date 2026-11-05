using AlekoDispatch.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlekoDispatch.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext(options)
    {
        public DbSet<WorkSite> WorkSites { get; set; } = null!;
        public DbSet<HeavyMachine> HeavyMachines { get; set; } = null!;
        public DbSet<DailyWorkLog> DailyWorkLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<HeavyMachine>()
                .HasOne(h => h.CurrentWorkSite)
                .WithMany(w => w.AssignedMachines)
                .HasForeignKey(h => h.CurrentWorkSiteId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<DailyWorkLog>()
                .HasOne(d => d.WorkSite)
                .WithMany(w => w.DailyWorkLogs)
                .HasForeignKey(d => d.WorkSiteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DailyWorkLog>()
                .HasOne(d => d.HeavyMachine)
                .WithMany(h => h.DailyWorkLogs)
                .HasForeignKey(d => d.HeavyMachineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
