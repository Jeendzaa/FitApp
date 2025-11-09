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
        public DailyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyReport>>> GetAllDailies()
        {
            var dailies = await _context.DailyReports
                .Include(d => d.User)
                .Include(d => d.MealEntries)
                .ToListAsync();

            return Ok(dailies);
        }

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

        [HttpGet("user/{userId}/date/{date}")]
        public async Task<ActionResult<DailyReport>> GetUserDailyByDate(int id, DateTime date)
        {
            var daily = await _context.DailyReports
                .Include (d => d.MealEntries)
                .FirstOrDefaultAsync(d => d.UserId == id && d.DailyReportDate.Date == date);

            if (daily == null)
                return NotFound($"No day {date: yyyy - MM - dd} found for user {id}.");

            return Ok(daily);
        }

        [HttpPost]
        public async Task<ActionResult<DailyReport>> CreateDaily([FromBody] DailyReport newDaily)
        {
            var user = await _context.Users.FindAsync(newDaily.UserId);
            if (user == null)
                return NotFound("User not found");

            var existing = await _context.DailyReports
                .FirstOrDefaultAsync(d => d.UserId == newDaily.UserId && d.DailyReportDate.Date == newDaily.DailyReportDate.Date);

            if (existing == null)
                return BadRequest("This day has already been created");

            _context.DailyReports.Add(newDaily);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserDailyByDate), new {userId = newDaily.UserId, date = newDaily.DailyReportDate}, newDaily);
        }

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
