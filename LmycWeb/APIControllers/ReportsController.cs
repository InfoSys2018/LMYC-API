using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using System.Dynamic;

namespace LmycWeb.APIControllers
{
    [Produces("application/json")]
    [Route("api/Reports")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reports
        [HttpGet]
        public IEnumerable<Object> GetReport()
        {
            List<Object> report = new List<Object>();
            foreach (Report r in _context.Report)
            {
                dynamic v = new ExpandoObject();
                //Get the username and classification details instead of IDs.
                v.User = _context.Users.Find(r.UserId).UserName;
                v.Classification = _context.ClassificationCodes.Find(r.CodeId).Classification;
                //v.classification = r.Code.Classification;

                v.CodeId = r.CodeId;
                v.Content = r.Content;
                v.Approved = r.Approved;
                v.Hours = r.Hours;
                v.DateCreated = r.DateCreated;

                report.Add(v);
            }
            return report;
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

            dynamic v = new ExpandoObject();
            //Get the username and classification details instead of IDs.
            v.User = _context.Users.Find(report.Id).UserName;
            v.Classification = _context.ClassificationCodes.Find(report.CodeId).Classification;
            v.CodeId = report.CodeId;
            v.Content = report.Content;
            v.Approved = report.Approved;
            v.Hours = report.Hours;
            v.DateCreatd = report.DateCreated;

            return Ok(v);
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
            report.Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

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