using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SNRS.Data;
using SNRS.Models;

namespace SNRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
        {
            return await _context.Reports
                .Include(r => r.Project)
                .Include(r => r.GeneratedBy)
                .ToListAsync();
        }

        // GET: api/reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _context.Reports
                .Include(r => r.Project)
                .Include(r => r.GeneratedBy)
                .FirstOrDefaultAsync(r => r.ReportID == id);

            if (report == null)
                return NotFound();

            return report;
        }

        // GET: api/reports/search?type=Progress
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Report>>> SearchReports([FromQuery] string type)
        {
            return await _context.Reports
                .Include(r => r.Project)
                .Where(r => r.ReportType.Contains(type))
                .ToListAsync();
        }

        // POST: api/reports
        [HttpPost]
        public async Task<ActionResult<Report>> CreateReport(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReport), new { id = report.ReportID }, report);
        }

        // PUT: api/reports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, Report report)
        {
            if (id != report.ReportID)
                return BadRequest();

            var existing = await _context.Reports.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.ProjectID = report.ProjectID;
            existing.GeneratedByID = report.GeneratedByID;
            existing.ReportType = report.ReportType;
            existing.Summary = report.Summary;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
                return NotFound();

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}