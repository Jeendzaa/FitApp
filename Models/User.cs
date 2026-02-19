namespace FitApp.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public string UserPassword { get; set; } = string.Empty;

        public DateTime UserDateOfBirth { get; set; }

        public int UserCurrentWeight { get; set; }

        public int UserBmi { get; set; }

        public int DailyCalorieGoal { get; set; }

        public ICollection<DailyReport> DailyReports { get; set; } = new List<DailyReport>();
    }
}
