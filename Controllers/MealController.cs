using FitApp.Data;
using FitApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : Controller
    {
        private readonly AppDbContext _context;

        public MealController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meal>>> GetAllMeals()
        {
            var meals = await _context.MealReports.ToListAsync();
            return Ok(meals);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Meal>> GetMealById(int id)
        {
            var meal = await _context.MealReports.FindAsync(id);
            if (meal == null)
                return NotFound("Meal not found");
            return Ok(meal);
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<IEnumerable<Meal>>> GetMealByName(string name)
        {
            var meals = await _context.MealReports
                .Where(m => m.MealName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
            if (meals.Count == 0)
                return NotFound("Meals not found");
            return Ok(meals);
                
        }

        [HttpPost]
        public async Task<ActionResult<Meal>> CreateMeal([FromBody] Meal newMeal)
        {
            if (string.IsNullOrWhiteSpace(newMeal.MealName))
                return BadRequest("Name of meal is empty");
            _context.MealReports.Add(newMeal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMealById), new {id = newMeal.MealId}, newMeal);
        }

        [HttpPut("id")]
        public async Task<IActionResult> EditMeal(int id, [FromBody] Meal updatedMeal)
        {
            if (id != updatedMeal.MealId)
                return BadRequest("The ID from the URL does not match the meal ID");

            var meal = await _context.MealReports.FindAsync(id);
            if (meal == null)
                return BadRequest("Meal not found");

            meal.MealName = updatedMeal.MealName;
            meal.MealCalories = updatedMeal.MealCalories;
            meal.MealProtein = updatedMeal.MealProtein;
            meal.MealCarbon = updatedMeal.MealCarbon;
            meal.MealFat = updatedMeal.MealFat;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<ActionResult<IEnumerable<Meal>>> DeleteMeal(int id)
        {
            var meal = await _context.MealEntries.FindAsync(id);
            if (meal == null)
                return BadRequest("Meal not found");

            _context.MealEntries.Remove(meal);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
