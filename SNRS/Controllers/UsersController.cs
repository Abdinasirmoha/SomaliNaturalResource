using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SNRS.Data;
using SNRS.Models;

namespace SNRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

        // POST: api/users  (Add new user)
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // Prevent duplicate username/email
            bool exists = await _context.Users
                .Where(u => u.Username == user.Username || u.Email == user.Email)
                .AnyAsync();

            if (exists)
                return BadRequest("Username or Email already exists.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.UserID)
                return BadRequest();

            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.FullName = user.FullName;
            existing.Email = user.Email;
            existing.Role = user.Role;
            existing.IsActive = user.IsActive;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Username == request.Username ||
                    u.Email == request.Username);

            if (user == null || user.PasswordHash != request.Password || !user.IsActive)
                return Unauthorized("Invalid username or password.");

            return Ok(new
            {
                user.UserID,
                user.FullName,
                user.Username,
                user.Email,
                user.Role
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}