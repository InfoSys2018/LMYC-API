using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LmycWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public IActionResult Index(string searchString)
        {
            List<Report> Reports;
            if (String.IsNullOrEmpty(searchString))
            {
                Reports = _context.Reports.Include(r => r.Code).Where(r => !r.Approved).ToList();
            }
            else
            {
                var user = _context.ApplicationUser.FirstOrDefault(u => u.UserName == searchString);
                try
                {
                    Reports = _context.Reports.Include(r => r.Code).Where(r => !r.Approved
                && r.UserId == user.Id).ToList();
                }
                catch (Exception)
                {
                    Reports = _context.Reports.Include(r => r.Code).Where(r => !r.Approved).ToList();
                }
            }
            return View(Reports);
        }

        public async Task<IActionResult> ViewAll(string searchString)
        {
            List<Report> Reports;
            if (String.IsNullOrEmpty(searchString))
            {
                Reports = await _context.Reports.Include(r => r.Code).ToListAsync();
            }
            else
            {
                var user = _context.ApplicationUser.FirstOrDefault(u => u.UserName == searchString);
                try
                {
                    Reports = await _context.Reports.Include(r => r.Code).Where(r => r.UserId == user.Id).ToListAsync();
                }
                catch (Exception)
                {
                    Reports = await _context.Reports.Include(r => r.Code).ToListAsync();
                }
            }
            return View (Reports);
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Code)
                .SingleOrDefaultAsync(m => m.ReportID == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["CodeId"] = new SelectList(_context.Set<ClassificationCode>(), "CodeId", "CodeId");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportID,Content,Hours,Approved,DateCreated,Id,UserId,CodeId")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodeId"] = new SelectList(_context.Set<ClassificationCode>(), "CodeId", "CodeId", report.CodeId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", report.UserId);
            return View(report);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ReportID == id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["CodeId"] = new SelectList(_context.Set<ClassificationCode>(), "CodeId", "CodeId", report.CodeId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", report.UserId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ReportID,Content,Hours,Approved,DateCreated,Id,UserId,CodeId")] Report report)

        {
            if (id != report.ReportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodeId"] = new SelectList(_context.Set<ClassificationCode>(), "CodeId", "CodeId", report.CodeId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", report.UserId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Code)
                .SingleOrDefaultAsync(m => m.ReportID == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ReportID == id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(string id)
        {
            return _context.Reports.Any(e => e.ReportID == id);
        }
    }
}
