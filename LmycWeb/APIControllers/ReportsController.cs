using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using LmycWeb.Interfaces;

namespace LmycWeb.APIControllers
{
    [Produces("application/json")]
    [Route("api/Reports")]
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [EnableCors("CorsPolicy")]
    public class ReportsController : Controller
    {
        private readonly IDbContext _context;

        public ReportsController(IDbContext context)
        {
            _context = context;
        }

        // GET: api/Reports
        [HttpGet]
        public IEnumerable<Report> GetReport()
        {
            return _context.Reports;
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ReportID == id);

            if (report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }

        // PUT: api/Reports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReport([FromRoute] string id, [FromBody] Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != report.ReportID)
            {
                return BadRequest();
            }

            _context.Entry(report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<IActionResult> PostReport([FromBody] Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReport", new { id = report.ReportID }, report);
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ReportID == id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return Ok(report);
        }

        private bool ReportExists(string id)
        {
            return _context.Reports.Any(e => e.ReportID == id);
        }
    }
}