namespace FitApp.Api.Models.DTO
{
    public class DailyReportDto
    {
        public int Id { get; set; }
        public DateTime DailyReportDate { get; set; }

        public int DailyReportCalories { get; set; }
        public double? DailyReportProtein { get; set; }
        public double? DailyReportCarbon { get; set; }
        public double? DailyReportFat { get; set; }
        public double? DailyReportWater { get; set; }

        public List<MealEntryDto> Meals { get; set; }
    }


    public class MealEntryDto
    {
        public int MealEntryId { get; set; }
        public int Quantity { get; set; }

        public string MealName { get; set; } = "";
        public int MealCalories { get; set; }
        public int MealProtein { get; set; }
        public int MealCarbon { get; set; }
        public int MealFat { get; set; }
    }
}
