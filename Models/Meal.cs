namespace FitApp.Models
{
    /// <summary>
    /// Represents a meal definition with nutritional information.
    /// Contains calories, macronutrients, and related meal entries.
    /// </summary>
    public class Meal
    {
        /// <summary>
        /// Unique identifier for the meal.
        /// </summary>
        public int MealId { get; set; }

        /// <summary>
        /// The name of the meal.
        /// </summary>
        public string MealName { get; set; }

        /// <summary>
        /// Total calories contained in the meal.
        /// </summary>
        public int MealCalories { get; set; }

        /// <summary>
        /// Protein content (grams) of the meal.
        /// </summary>
        public int MealProtein { get; set; }

        /// <summary>
        /// Carbohydrate content (grams) of the meal.
        /// </summary>
        public int MealCarbon { get; set; }

        /// <summary>
        /// Fat content (grams) of the meal.
        /// </summary>
        public int MealFat { get; set; }

        /// <summary>
        /// Collection of meal entries that reference this meal.
        /// </summary>
        public ICollection<MealEntry> MealEntries { get; set; } = new List<MealEntry>();
    }
}
