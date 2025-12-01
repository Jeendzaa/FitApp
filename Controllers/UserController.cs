using FitApp.Data;
using FitApp.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public readonly AppDbContext _context;

        public UserController(AppDbContext context) { _context = context ; }

        //GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/id
        [HttpGet("id")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found!");
            return Ok(user);
        }

        // GET: api/users/by-name/{name}
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserByName(string name)
        {
            var users = await _context.Users
                .Where(u => u.UserName.Contains(name))
                .ToListAsync();

            if (!users.Any()) return NotFound("User not found!");
            return Ok(users);
        }

        // GET: api/users/by-mail/{mail}
        [HttpGet("by-mail/{mail}")]
        public async Task<ActionResult<User>> GetUserByMail(string name)
        {
            var users = await _context.Users
                .Where(u => u.UserEmail.Contains(name))
                .ToListAsync();

            if (!users.Any()) return NotFound("User not found!");
            return Ok(users);
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User newUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == newUser.UserEmail);
            if ( existingUser != null ) return BadRequest("Email is taken");
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new {id = newUser.UserId}, newUser);
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser([FromBody] LoginRequest loginData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == loginData.Email && u.UserPassword == loginData.Password);
            if (user == null) return BadRequest("Login data is incorrect");
            return Ok(user);
        }

        // PUT /api/user/{id}
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

        // DELETE: /api/user/{id}
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
