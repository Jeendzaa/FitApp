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

        /// <summary>
        /// Controller for managing meal entries.
        /// Provides endpoints to create, read, update, and delete meal entries.
        /// </summary>
        public MealEntryController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all meal entries with related meal and daily report data.
        /// </summary>
        /// <returns>A list of <see cref="MealEntry"/> objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealEntry>>> GetMealEntries()
        {
            var mealEntries = await _context.MealEntries
                .Include(me => me.Meal)
                .Include(me => me.DailyReport)
                .ToListAsync();

            return Ok(mealEntries);
        }

        /// <summary>
        /// Retrieves all meal entries associated with a specific daily report.
        /// </summary>
        /// <param name="dailyId">The ID of the daily report.</param>
        /// <returns>A list of <see cref="MealEntry"/> objects for the given daily report.</returns>
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

        /// <summary>
        /// Retrieves a specific meal entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the meal entry.</param>
        /// <returns>The <see cref="MealEntry"/> object if found, otherwise 404 Not Found.</returns>
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

        /// <summary>
        /// Creates a new meal entry linked to a daily report and a meal.
        /// </summary>
        /// <param name="newMeal">The new <see cref="MealEntry"/> object to create.</param>
        /// <returns>The created <see cref="MealEntry"/> object with its ID.</returns>
        [HttpPost]
        public async Task<ActionResult<MealEntry>> CreateMealEntry([FromBody] MealEntry newMeal)
        {
            var daily = await _context.DailyReports.FindAsync(newMeal.DailyReportId);
            if (daily == null)
                return BadRequest("Daily not found");

            var meal = await _context.Meals.FindAsync(newMeal.MealId);
            if (meal == null)
                return BadRequest("Meal not found");

            _context.MealEntries.Add(newMeal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateMealEntry), new {id = newMeal.MealEntryId}, newMeal);
        }

        /// <summary>
        /// Updates an existing meal entry.
        /// </summary>
        /// <param name="id">The ID of the meal entry to update.</param>
        /// <param name="updatedEntry">The updated <see cref="MealEntry"/> object.</param>
        /// <returns>No content if successful, otherwise 400 or 404 error.</returns>
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

        /// <summary>
        /// Deletes a meal entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the meal entry to delete.</param>
        /// <returns>No content if successful, otherwise 404 error if not found.</returns>
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
