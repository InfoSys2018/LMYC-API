using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;

namespace LmycWeb.APIControllers
{
    [Produces("application/json")]
    [Route("api/BoatsApi")]
    [EnableCors("AllowAllOrigins")]
    [Authorize(Policy = "LoginRequired", AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class BoatsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BoatsApi
        [HttpGet]
        public IEnumerable<Boat> GetBoats()
        {
            return _context.Boats;
        }

        // GET: api/BoatsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoat([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boat = await _context.Boats.SingleOrDefaultAsync(m => m.BoatId == id);

            if (boat == null)
            {
                return NotFound();
            }

            return Ok(boat);
        }

        // PUT: api/BoatsApi/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> PutBoat([FromRoute] string id, [FromBody] Boat boat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != boat.BoatId)
            {
                return BadRequest();
            }

            _context.Entry(boat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoatExists(id))
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

        // POST: api/BoatsApi
        [HttpPost]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> PostBoat([FromBody] Boat boat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Boats.Add(boat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoat", new { id = boat.BoatId }, boat);
        }

        // DELETE: api/BoatsApi/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminRequired")]
        public async Task<IActionResult> DeleteBoat([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boat = await _context.Boats.SingleOrDefaultAsync(m => m.BoatId == id);
            if (boat == null)
            {
                return NotFound();
            }

            _context.Boats.Remove(boat);
            await _context.SaveChangesAsync();

            return Ok(boat);
        }

        private bool BoatExists(string id)
        {
            return _context.Boats.Any(e => e.BoatId == id);
        }
    }
}