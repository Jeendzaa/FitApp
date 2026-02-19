using FitApp.Api.DTO;
using FitApp.Data;
using FitApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class MealEntryController : Controller
    {
        private readonly AppDbContext _context;

        public MealEntryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MealEntry
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealEntry>>> GetMealEntries()
        {
            var mealEntries = await _context.MealEntries
                .Include(me => me.Meal) // wystarczy Meal, Daily powoduje cykle
                .ToListAsync();

            return Ok(mealEntries);
        }

        // GET: api/MealEntry/daily/5  ← KLUCZOWY endpoint dla Twojej apki
        [HttpGet("daily/{dailyId}")]
        public async Task<ActionResult<IEnumerable<MealEntry>>> GetMealEntryByDaily(int dailyId)
        {
            var mealEntries = await _context.MealEntries
                .Include(me => me.Meal)
                .Where(me => me.DailyReportId == dailyId)
                .ToListAsync();

            if (mealEntries.Count == 0)
                return NotFound("Meal entry not found");

            return Ok(mealEntries);
        }

        // GET: api/MealEntry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MealEntry>> GetMealEntryById(int id)
        {
            var mealEntry = await _context.MealEntries
                .Include(me => me.Meal)
                .FirstOrDefaultAsync(me => me.MealEntryId == id); // POPRAWKA: wcześniej brak filtra

            if (mealEntry == null)
                return NotFound("Meal entry not found");

            return Ok(mealEntry);
        }

        // POST: api/MealEntry
        [HttpPost]
        public async Task<IActionResult> CreateMealEntry([FromBody] CreateMealEntryDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data");

            var daily = await _context.DailyReports
                .FirstOrDefaultAsync(d => d.Id == dto.DailyReportId);

            if (daily == null)
                return NotFound("DailyReport not found");

            var meal = await _context.Meals
                .FirstOrDefaultAsync(m => m.MealId == dto.MealId);

            if (meal == null)
                return NotFound("Meal not found");

            // 1. Dodanie wpisu
            var mealEntry = new MealEntry
            {
                DailyReportId = dto.DailyReportId,
                MealId = dto.MealId,
                MealEntryQuantity = dto.MealEntryQuantity,
                MealEntryDate = dto.MealEntryDate
            };

            _context.MealEntries.Add(mealEntry);

            // 2. ZABEZPIECZENIE NA NULL (KLUCZ!)
            daily.DailyReportProtein ??= 0;
            daily.DailyReportCarbon ??= 0;
            daily.DailyReportFat ??= 0;

            var qty = dto.MealEntryQuantity;

            // 3. Aktualizacja sum dziennych (z bazy Meal, NIE z navigation)
            daily.DailyReportCalories += meal.MealCalories * qty;
            daily.DailyReportProtein += meal.MealProtein * qty;
            daily.DailyReportCarbon += meal.MealCarbon * qty;
            daily.DailyReportFat += meal.MealFat * qty;

            await _context.SaveChangesAsync();

            return Ok(mealEntry.MealEntryId);
        }

        // PUT: api/MealEntry/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditMealEntry(int id, [FromBody] MealEntry updatedEntry)
        {
            if (id != updatedEntry.MealEntryId)
                return BadRequest("ID mismatch");

            var existingEntry = await _context.MealEntries.FindAsync(id);
            if (existingEntry == null)
                return NotFound("Entry not found");

            existingEntry.MealId = updatedEntry.MealId;
            existingEntry.DailyReportId = updatedEntry.DailyReportId;
            existingEntry.MealEntryQuantity = updatedEntry.MealEntryQuantity;

            await _context.SaveChangesAsync();
            return Ok(existingEntry);
        }

        // DELETE: api/MealEntry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMealEntry(int id)
        {
            var entry = await _context.MealEntries.FindAsync(id);
            if (entry == null)
                return NotFound("Entry not found");

            _context.MealEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
