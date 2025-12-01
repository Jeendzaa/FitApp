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

        /// <summary>
        /// Initializes a new instance of the <see cref="MealController"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing meals data.</param>
        public MealController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all meals from the database.
        /// </summary>
        /// <returns>A list of <see cref="Meal"/> objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meal>>> GetAllMeals()
        {
            var meals = await _context.Meals.ToListAsync();
            return Ok(meals);
        }

        /// <summary>
        /// Retrieves a specific meal based on its ID.
        /// </summary>
        /// <param name="id">Meal ID.</param>
        /// <returns>A <see cref="Meal"/> object if found, otherwise 404 Not Found.</returns>
        [HttpGet("id")]
        public async Task<ActionResult<Meal>> GetMealById(int id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal == null)
                return NotFound("Meal not found");
            return Ok(meal);
        }

        /// <summary>
        /// Retrieves meals that match a given name (case-insensitive).
        /// </summary>
        /// <param name="name">The name or partial name of the meal.</param>
        /// <returns>A list of matching <see cref="Meal"/> objects.</returns>
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<IEnumerable<Meal>>> GetMealByName(string name)
        {
            var meals = await _context.Meals
                .Where(m => m.MealName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
            if (meals.Count == 0)
                return NotFound("Meals not found");
            return Ok(meals);
        }

        /// <summary>
        /// Creates a new meal entry.
        /// </summary>
        /// <param name="newMeal">The new <see cref="Meal"/> object to create.</param>
        /// <returns>The created <see cref="Meal"/> object with its ID.</returns>
        [HttpPost]
        public async Task<ActionResult<Meal>> CreateMeal([FromBody] Meal newMeal)
        {
            if (string.IsNullOrWhiteSpace(newMeal.MealName))
                return BadRequest("Name of meal is empty");
            _context.Meals.Add(newMeal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMealById), new { id = newMeal.MealId }, newMeal);
        }

        /// <summary>
        /// Updates an existing meal entry.
        /// </summary>
        /// <param name="id">The ID of the meal to update.</param>
        /// <param name="updatedMeal">The updated <see cref="Meal"/> object.</param>
        /// <returns>No content if successful, otherwise 400 or 404 error.</returns>
        [HttpPut("id")]
        public async Task<IActionResult> EditMeal(int id, [FromBody] Meal updatedMeal)
        {
            if (id != updatedMeal.MealId)
                return BadRequest("The ID from the URL does not match the meal ID");

            var meal = await _context.Meals.FindAsync(id);
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

        /// <summary>
        /// Deletes a meal entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the meal to delete.</param>
        /// <returns>No content if successful, otherwise 400 error if not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal == null)
                return BadRequest("Meal not found");

            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
