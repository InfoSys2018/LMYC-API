using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;

namespace LmycWeb.APIControllers
{
    [Produces("application/json")]
    [Route("api/classificationcodes")]
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class ClassificationCodesApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassificationCodesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ClassificationCodesApi
        [HttpGet]
        public IEnumerable<ClassificationCode> GetClassificationCodes()
        {
            return _context.ClassificationCodes;
        }

        // GET: api/ClassificationCodesApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassificationCode([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var classificationCode = await _context.ClassificationCodes.SingleOrDefaultAsync(m => m.CodeId == id);

            if (classificationCode == null)
            {
                return NotFound();
            }

            return Ok(classificationCode);
        }

        // PUT: api/ClassificationCodesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassificationCode([FromRoute] string id, [FromBody] ClassificationCode classificationCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != classificationCode.CodeId)
            {
                return BadRequest();
            }

            _context.Entry(classificationCode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassificationCodeExists(id))
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

        // POST: api/ClassificationCodesApi
        [HttpPost]
        public async Task<IActionResult> PostClassificationCode([FromBody] ClassificationCode classificationCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ClassificationCodes.Add(classificationCode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassificationCode", new { id = classificationCode.CodeId }, classificationCode);
        }

        // DELETE: api/ClassificationCodesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassificationCode([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var classificationCode = await _context.ClassificationCodes.SingleOrDefaultAsync(m => m.CodeId == id);
            if (classificationCode == null)
            {
                return NotFound();
            }

            _context.ClassificationCodes.Remove(classificationCode);
            await _context.SaveChangesAsync();

            return Ok(classificationCode);
        }

        private bool ClassificationCodeExists(string id)
        {
            return _context.ClassificationCodes.Any(e => e.CodeId == id);
        }
    }
}