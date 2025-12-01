using FitApp.Data;
using FitApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public readonly AppDbContext _context;

        /// <summary>
        /// Controller for managing users.
        /// Provides endpoints to register, login, retrieve, update, and delete users.
        /// </summary>
        public UserController(AppDbContext context) { _context = context ; }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A list of <see cref="User"/> objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The <see cref="User"/> object if found, otherwise 404 Not Found.</returns>
        [HttpGet("id")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found!");
            return Ok(user);
        }

        /// <summary>
        /// Retrieves users whose names contain the given string.
        /// </summary>
        /// <param name="name">The name or partial name to search for.</param>
        /// <returns>A list of matching <see cref="User"/> objects.</returns>
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserByName(string name)
        {
            var users = await _context.Users
                .Where(u => u.UserName.Contains(name))
                .ToListAsync();

            if (!users.Any()) return NotFound("User not found!");
            return Ok(users);
        }

        /// <summary>
        /// Retrieves users whose email addresses contain the given string.
        /// </summary>
        /// <param name="mail">The email or partial email to search for.</param>
        /// <returns>A list of matching <see cref="User"/> objects.</returns>
        [HttpGet("by-mail/{mail}")]
        public async Task<ActionResult<User>> GetUserByMail(string name)
        {
            var users = await _context.Users
                .Where(u => u.UserEmail.Contains(name))
                .ToListAsync();

            if (!users.Any()) return NotFound("User not found!");
            return Ok(users);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="newUser">The new <see cref="User"/> object to create.</param>
        /// <returns>The created <see cref="User"/> object with its ID.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User newUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == newUser.UserEmail);
            if ( existingUser != null ) return BadRequest("Email is taken");
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new {id = newUser.UserId}, newUser);
        }

        /// <summary>
        /// Authenticates a user with email and password.
        /// </summary>
        /// <param name="loginData">The login request containing email and password.</param>
        /// <returns>The authenticated <see cref="User"/> object if credentials are valid, otherwise 400 Bad Request.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser([FromBody] LoginRequest loginData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == loginData.Email && u.UserPassword == loginData.Password);
            if (user == null) return BadRequest("Login data is incorrect");
            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updatedUser">The updated <see cref="User"/> object.</param>
        /// <returns>No content if successful, otherwise 400 or 404 error.</returns>
        [HttpPut("id")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            if (id != updatedUser.UserId) return BadRequest("Bad user ID");
            _context.Entry(updatedUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.UserId == id))
                    return NotFound("No user to update");
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content if successful, otherwise 404 error if not found.</returns>
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
