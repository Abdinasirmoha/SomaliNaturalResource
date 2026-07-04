using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SNRS.Data;
using SNRS.Models;

namespace SNRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects
                .Include(p => p.Resource)
                    .ThenInclude(r => r!.Category)
                .ToListAsync();
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Resource)
                    .ThenInclude(r => r!.Category)
                .FirstOrDefaultAsync(p => p.ProjectID == id);

            if (project == null)
                return NotFound();

            return project;
        }

        // GET: api/projects/search?name=oil
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Project>>> SearchProjects([FromQuery] string name)
        {
            return await _context.Projects
                .Include(p => p.Resource)
                .Where(p => p.ProjectName.Contains(name) || p.CompanyName.Contains(name))
                .ToListAsync();
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectID }, project);
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.ProjectID)
                return BadRequest();

            var existing = await _context.Projects.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.ProjectName = project.ProjectName;
            existing.ResourceID = project.ResourceID;
            existing.CompanyName = project.CompanyName;
            existing.StartDate = project.StartDate;
            existing.EndDate = project.EndDate;
            existing.Status = project.Status;
            existing.Description = project.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}