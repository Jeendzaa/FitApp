namespace FitApp.Api.DTO
{
    public class CreateMealEntryDto
    {
        public int DailyReportId { get; set; }
        public int MealId { get; set; }
        public int MealEntryQuantity { get; set; }
        public DateTime MealEntryDate { get; set; }
    }
}
