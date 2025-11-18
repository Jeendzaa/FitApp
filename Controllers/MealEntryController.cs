using FitApp.Data;
using FitApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class MealEntryController : Controller
    {
        private readonly AppDbContext _context;
        public MealEntryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealEntry>>> GetMealEntries()
        {
            var mealEntries = await _context.MealEntries
                .Include(me => me.Meal)
                .Include(me => me.DailyReport)
                .ToListAsync();

            return Ok(mealEntries);
        }

        [HttpGet("daily/{dailyId}")]
        public async Task<ActionResult<IEnumerable<MealEntry>>> GetMealEntryByDaily(int id)
        {
            var mealEntry = await _context.MealEntries
                .Include(me => me.Meal)
                .Include(me => me.DailyReport)
                .ToListAsync();

            if (mealEntry.Count == 0)
                return NotFound("Meal entry not found");

            return Ok(mealEntry);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealEntry>> GetMealEntryById(int id)
        {
            var mealEntry = await _context.MealEntries
                .Include (me => me.Meal)
                .Include(me => me.DailyReport)
                .FirstOrDefaultAsync();

            if (mealEntry == null)
                return NotFound("Meal entry not found");

            return Ok(mealEntry);
        }

        [HttpPost]
        public async Task<ActionResult<MealEntry>> CreateMealEntry([FromBody] MealEntry newMeal)
        {
            var daily = await _context.DailyReports.FindAsync(newMeal.DailyReportId);
            if (daily == null)
                return BadRequest("Daily not found");

            var meal = await _context.MealReports.FindAsync(newMeal.MealId);
            if (meal == null)
                return BadRequest("Meal not found");

            _context.MealEntries.Add(newMeal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateMealEntry), new {id = newMeal.MealEntryId}, newMeal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMealEntry(int id, [FromBody] MealEntry updatedEntry)
        {
            if (id != updatedEntry.MealEntryId)
                return BadRequest("The ID in the address does not match the ID of the entry");

            var existingEntry = await _context.MealEntries.FindAsync(id);
            if (existingEntry == null) 
                return NotFound("Entry not found");

            existingEntry.MealId = updatedEntry.MealId;
            existingEntry.DailyReportId = updatedEntry.DailyReportId;
            existingEntry.MealEntryQuantity = updatedEntry.MealEntryQuantity;
            existingEntry.MealEntryDate = updatedEntry.MealEntryDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMealEntry(int id)
        {
            var entry = await _context.MealEntries.FindAsync(id);
            if (entry == null)
                return NotFound("Entry not found");

            _context.MealEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
