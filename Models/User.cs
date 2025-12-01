namespace FitApp.Models
{
    /// <summary>
    /// Represents an application user with personal information and related daily reports.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The username chosen by the user.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// The password of the user (stored as plain text here, should be hashed in production).
        /// </summary>
        public string UserPassword { get; set; } = string.Empty;

        /// <summary>
        /// The date of birth of the user.
        /// </summary>
        public DateTime UserDateOfBirth { get; set; }

        /// <summary>
        /// The current weight of the user (in kilograms).
        /// </summary>
        public int UserCurrentWeight { get; set; }

        /// <summary>
        /// The Body Mass Index (BMI) of the user.
        /// </summary>
        public int UserBmi { get; set; }

        /// <summary>
        /// Collection of daily reports associated with the user.
        /// </summary>
        public ICollection<DailyReport> DailyReports { get; set; } = new List<DailyReport>();
    }
}
