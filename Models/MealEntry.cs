namespace FitApp.Models
{
    /// <summary>
    /// Represents a single meal entry linked to a daily report.
    /// Contains information about the meal, quantity, and date.
    /// </summary>
    public class MealEntry
    {
        /// <summary>
        /// Unique identifier for the meal entry.
        /// </summary>
        public int MealEntryId { get; set; }

        /// <summary>
        /// Quantity of the meal consumed.
        /// </summary>
        public int MealEntryQuantity { get; set; }

        /// <summary>
        /// The date when the meal entry was recorded.
        /// </summary>
        public DateTime MealEntryDate { get; set; }

        /// <summary>
        /// The ID of the associated meal.
        /// </summary>
        public int MealId { get; set; }

        /// <summary>
        /// Navigation property to the associated meal.
        /// </summary>
        public Meal Meal { get; set; }

        /// <summary>
        /// The ID of the associated daily report.
        /// </summary>
        public int DailyReportId { get; set; }

        /// <summary>
        /// Navigation property to the associated daily report.
        /// </summary>
        public DailyReport DailyReport { get; set; }
    }
}
