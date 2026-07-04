using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SNRS.Data;
using SNRS.Models;

namespace SNRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResourcesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/resources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            return await _context.Resources
                .Include(r => r.Category)
                .ToListAsync();
        }

        // GET: api/resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(int id)
        {
            var resource = await _context.Resources
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.ResourceID == id);

            if (resource == null)
                return NotFound();

            return resource;
        }

        // GET: api/resources/search?name=oil
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Resource>>> SearchResources([FromQuery] string name)
        {
            var results = await _context.Resources
                .Include(r => r.Category)
                .Where(r => r.ResourceName.Contains(name))
                .ToListAsync();

            return results;
        }

        // POST: api/resources
        [HttpPost]
        public async Task<ActionResult<Resource>> CreateResource(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResource), new { id = resource.ResourceID }, resource);
        }

        // PUT: api/resources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResource(int id, Resource resource)
        {
            if (id != resource.ResourceID)
                return BadRequest();

            var existing = await _context.Resources.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.ResourceName = resource.ResourceName;
            existing.CategoryID = resource.CategoryID;
            existing.Location = resource.Location;
            existing.Quantity = resource.Quantity;
            existing.Unit = resource.Unit;
            existing.Status = resource.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
                return NotFound();

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}