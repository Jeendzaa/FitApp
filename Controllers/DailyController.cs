using FitApp.Api.Models.DTO;
using FitApp.Data;
using FitApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyController : Controller
    {
        private readonly AppDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyController"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing application data.</param>
        public DailyController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all daily reports with related user and meal entries.
        /// </summary>
        /// <returns>A list of <see cref="DailyReport"/> objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyReport>>> GetAllDailies()
        {
            var dailies = await _context.DailyReports
                .Include(d => d.User)
                .Include(d => d.MealEntries)
                .ToListAsync();

            return Ok(dailies);
        }

        /// <summary>
        /// Retrieves all daily reports for a specific user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>A list of <see cref="DailyReport"/> objects for the given user.</returns>
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<DailyReport>>> GetUserDailies(int id)
        {
            var dailies = await _context.DailyReports
                .Where(d => d.UserId == id)
                .Include(d => d.MealEntries)
                .ToListAsync();

            if (dailies.Count == 0)
                return NotFound($"User {id} has no daily reports");

            return Ok(dailies);
        }

        /// <summary>
        /// Retrieves a daily report for a specific user on a given date.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="date">The date of the daily report.</param>
        /// <returns>A <see cref="DailyReport"/> object if found.</returns>
        [HttpGet("user/{userId}/date/{date}")]
        public async Task<ActionResult<DailyReportDto>> GetUserDailyByDate(int userId, DateTime date)
        {
            var daily = await _context.DailyReports
                .Include(d => d.MealEntries)
                    .ThenInclude(me => me.Meal)
                .FirstOrDefaultAsync(d =>
                    d.UserId == userId &&
                    d.DailyReportDate.Date == date.Date);

            if (daily == null)
                return NotFound();

            int totalCalories = 0;
            double totalProtein = 0;
            double totalCarbon = 0;
            double totalFat = 0;

            foreach (var entry in daily.MealEntries)
            {
                if (entry.Meal == null)
                    continue;

                var qty = entry.MealEntryQuantity;

                totalCalories += entry.Meal.MealCalories * qty;
                totalProtein += entry.Meal.MealProtein * qty;
                totalCarbon += entry.Meal.MealCarbon * qty;
                totalFat += entry.Meal.MealFat * qty;
            }

            daily.DailyReportCalories = totalCalories;
            daily.DailyReportProtein = totalProtein;
            daily.DailyReportCarbon = totalCarbon;
            daily.DailyReportFat = totalFat;

            await _context.SaveChangesAsync();

            var dto = new DailyReportDto
            {
                Id = daily.Id,
                DailyReportDate = daily.DailyReportDate,
                DailyReportCalories = totalCalories,
                DailyReportProtein = totalProtein,
                DailyReportCarbon = totalCarbon,
                DailyReportFat = totalFat,
                DailyReportWater = daily.DailyReportWater,

                Meals = daily.MealEntries
                    .Where(me => me.Meal != null) 
                    .Select(me => new MealEntryDto
                    {
                        MealEntryId = me.MealEntryId,
                        Quantity = me.MealEntryQuantity,
                        MealName = me.Meal.MealName,
                        MealCalories = me.Meal.MealCalories,
                        MealProtein = me.Meal.MealProtein,
                        MealCarbon = me.Meal.MealCarbon,
                        MealFat = me.Meal.MealFat
                    }).ToList()
            };

            return Ok(dto);
        }



        /// <summary>
        /// Creates a new daily report for a user.
        /// </summary>
        /// <param name="newDaily">The new daily report object.</param>
        /// <returns>The created <see cref="DailyReport"/> object.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDaily([FromBody] CreateDailyReportDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found");

            var existing = await _context.DailyReports
                .FirstOrDefaultAsync(d =>
                    d.UserId == dto.UserId &&
                    d.DailyReportDate.Date == dto.DailyReportDate.Date);

            if (existing != null)
                return BadRequest("This day already exists");

            var daily = new DailyReport
            {
                UserId = dto.UserId,
                DailyReportDate = dto.DailyReportDate,
                DailyReportCalories = 0
            };

            _context.DailyReports.Add(daily);
            await _context.SaveChangesAsync();

            return Ok(daily.Id);
        }



        /// <summary>
        /// Creates a new daily report for a user.
        /// </summary>
        /// <param name="newDaily">The new daily report object.</param>
        /// <returns>The created <see cref="DailyReport"/> object.</returns>
        [HttpPut]
        public async Task<IActionResult> EditDaily(int id, [FromBody] DailyReport updatedDaily)
        {
            if (id != updatedDaily.Id)
                return BadRequest("The D in the address does not match the ID of the day");

            var daily = await _context.DailyReports.FindAsync(id);
            if (daily == null)
                return NotFound("Daily not found");

            daily.DailyReportCalories = updatedDaily.DailyReportCalories;
            daily.DailyReportProtein = updatedDaily.DailyReportProtein;
            daily.DailyReportCarbon = updatedDaily.DailyReportCarbon;
            daily.DailyReportFat = updatedDaily.DailyReportFat;
            daily.DailyReportWater = updatedDaily.DailyReportWater;

            await _context.SaveChangesAsync();
            return Ok(daily);
        }

        [HttpPut("{id}/water")]
        public async Task<IActionResult> UpdateWater(int id, [FromBody] UpdateWaterDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data");

            var daily = await _context.DailyReports.FindAsync(id);
            if (daily == null)
                return NotFound("Daily not found");

            daily.DailyReportWater = dto.DailyReportWater;

            await _context.SaveChangesAsync();
            return Ok(daily);
        }

        /// <summary>
        /// Deletes a daily report by ID.
        /// </summary>
        /// <param name="id">The ID of the daily report to delete.</param>
        /// <returns>The deleted <see cref="DailyReport"/> object.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDaily(int id)
        {
            var daily = await _context.DailyReports.FindAsync(id);
            if (daily == null)
                return NotFound("Daily not found");

            _context.DailyReports.Remove(daily);
            await _context.SaveChangesAsync();
            return Ok(daily);
        }
    }
}
