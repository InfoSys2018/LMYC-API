using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using LmycWeb.Data;
using LmycWeb.Models;
using LmycWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LmycWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _services;

        public DocumentsController(ApplicationDbContext context, IServiceProvider services)
        {
            _context = context;
            _services = services;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Documents.ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .SingleOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentViewModel dvm)
        {
            Document document = new Document();
            if (ModelState.IsValid)
            {

                var userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
                document.User = userManager.GetUserAsync(HttpContext.User).Result;

                using (var memoryStream = new MemoryStream()) {
                    document.ContentType = dvm.Content.ContentType;
                    document.DocumentName = dvm.Content.FileName;
                    await dvm.Content.CopyToAsync(memoryStream);
                    document.Content = memoryStream.ToArray();
                }

                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .SingleOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(m => m.DocumentId == id);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(string id)
        {
            return _context.Documents.Any(e => e.DocumentId == id);
        }

        public IActionResult Download(string id) {

            if (id == null) {
                return NotFound();
            }

            Document doc = _context.Documents.FindAsync(id).Result;

            return File(doc.Content, doc.ContentType, doc.DocumentName);
        }
    }
}
