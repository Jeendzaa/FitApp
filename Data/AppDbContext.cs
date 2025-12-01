using FitApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealEntry> MealEntries {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.DailyReports)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyReport>()
                .HasMany(d => d.MealEntries)
                .WithOne(me => me.DailyReport)
                .HasForeignKey(me => me.DailyReportId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Meal>()
                .HasMany(m => m.MealEntries)
                .WithOne(me => me.Meal)
                .HasForeignKey(me => me.MealId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
