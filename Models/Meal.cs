namespace FitApp.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        public string MealName { get; set; }

        public int MealCalories { get; set; }
        public int MealProtein { get; set; }
        public int MealCarbon { get; set; }
        public int MealFat { get; set; }

        public ICollection<MealEntry> MealEntries { get; set; } = new List<MealEntry>();
    }
}
