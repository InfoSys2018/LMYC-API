using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using LmycWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using LmycWeb.Interfaces;

namespace LmycWeb.APIControllers
{
    [Produces("application/json")]
    [Route("api/documents")]
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [EnableCors("AllowAllOrigins")]
    public class DocumentsAPIController : Controller
    {
        private readonly IDbContext _context;
        private readonly IServiceProvider _services;

        public DocumentsAPIController(IDbContext context, IServiceProvider services)
        {
            _context = context;
            _services = services;
        }

        // GET: api/Documents
        [HttpGet]
        public IEnumerable<Document> GetDocuments()
        {
            return _context.Documents;
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var document = await _context.Documents.SingleOrDefaultAsync(m => m.DocumentId == id);

            if (document == null)
            {
                return NotFound();
            }

            return Ok(document);
        }

        // PUT: api/Documents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument([FromRoute] string id, [FromBody] Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.DocumentId)
            {
                return BadRequest();
            }

            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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

        // POST: api/Documents
        [HttpPost]
        public async Task<IActionResult> PostDocument([FromBody] DocumentViewModel dvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Document document = new Document();

            var userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
            document.User = userManager.GetUserAsync(HttpContext.User).Result;

            using (var memoryStream = new MemoryStream()) {
                document.ContentType = dvm.Content.ContentType;
                document.DocumentName = dvm.Content.FileName;
                await dvm.Content.CopyToAsync(memoryStream);
                document.Content = memoryStream.ToArray();
            }

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocument", new { id = document.DocumentId }, document);
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var document = await _context.Documents.SingleOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return Ok(document);
        }

        private bool DocumentExists(string id)
        {
            return _context.Documents.Any(e => e.DocumentId == id);
        }
    }
}