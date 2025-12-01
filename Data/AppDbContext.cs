using FitApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Data
{
    /// <summary>
    /// Application database context for FitApp.
    /// Manages entity sets and relationships between Users, DailyReports, Meals, and MealEntries.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the database context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the collection of users in the application.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the collection of daily reports in the application.
        /// </summary>
        public DbSet<DailyReport> DailyReports { get; set; }

        /// <summary>
        /// Gets or sets the collection of meals in the application.
        /// </summary>
        public DbSet<Meal> Meals { get; set; }

        /// <summary>
        /// Gets or sets the collection of meal entries in the application.
        /// </summary>
        public DbSet<MealEntry> MealEntries { get; set; }

        /// <summary>
        /// Configures entity relationships and constraints for the database model.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entities.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → DailyReports (cascade delete)
            modelBuilder.Entity<User>()
                .HasMany(u => u.DailyReports)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // DailyReport → MealEntries (cascade delete)
            modelBuilder.Entity<DailyReport>()
                .HasMany(d => d.MealEntries)
                .WithOne(me => me.DailyReport)
                .HasForeignKey(me => me.DailyReportId)
                .OnDelete(DeleteBehavior.Cascade);

            // Meal → MealEntries (restrict delete)
            modelBuilder.Entity<Meal>()
                .HasMany(m => m.MealEntries)
                .WithOne(me => me.Meal)
                .HasForeignKey(me => me.MealId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
