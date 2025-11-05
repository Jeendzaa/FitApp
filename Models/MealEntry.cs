namespace FitApp.Models
{
    public class MealEntry
    {
        public int MealEntryId { get; set; }

        public int MealEntryQuantity { get; set; }
        public DateTime MealEntryDate { get; set; }

        public int MealId { get; set; }
        public Meal Meal {  get; set; }

        public int DailyReportId {  get; set; }
        public DailyReport DailyReport { get; set; }
    }
}
