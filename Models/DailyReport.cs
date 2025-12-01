namespace FitApp.Models
{
    /// <summary>
    /// Represents a daily health and nutrition report for a user.
    /// Contains information about meals, calories, macronutrients, water intake, and weight.
    /// </summary>
    public class DailyReport
    {
        /// <summary>
        /// Unique identifier for the daily report.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date of the daily report.
        /// </summary>
        public DateTime DailyReportDate { get; set; }

        /// <summary>
        /// The ID of the user associated with this report.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the user who owns this report.
        /// </summary>
        public User User { get; set; } = null;

        /// <summary>
        /// Collection of meal entries linked to this daily report.
        /// </summary>
        public ICollection<MealEntry> MealEntries { get; set; } = new List<MealEntry>();

        /// <summary>
        /// Total calories consumed during the day.
        /// </summary>
        public int DailyReportCalories { get; set; }

        /// <summary>
        /// Total fat intake (grams) for the day.
        /// </summary>
        public double? DailyReportFat { get; set; }

        /// <summary>
        /// Total carbohydrate intake (grams) for the day.
        /// </summary>
        public double? DailyReportCarbon { get; set; }

        /// <summary>
        /// Total protein intake (grams) for the day.
        /// </summary>
        public double? DailyReportProtein { get; set; }

        /// <summary>
        /// Total water intake (liters) for the day.
        /// </summary>
        public double? DailyReportWater { get; set; }

        /// <summary>
        /// User's body weight recorded for the day.
        /// </summary>
        public double DailyReportWeight { get; set; }
    }
}
