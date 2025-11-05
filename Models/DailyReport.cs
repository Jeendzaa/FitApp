namespace FitApp.Models
{
    public class DailyReport
    {
        public int Id { get; set; }
        public DateTime DailyReportDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null;

        public ICollection<MealEntry> MealEntries { get; set; } = new List<MealEntry>();

        public int DailyReportCalories { get; set; }
        public double? DailyReportFat { get; set; }
        public double? DailyReportCarbon { get; set; }
        public double? DailyReportProtein { get; set; }
        public double? DailyReportWater { get; set; }
        public double DailyReportWeight { get; set; }
    }
}
