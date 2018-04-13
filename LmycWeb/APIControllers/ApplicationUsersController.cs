using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace LmycWeb.APIControllers
{
    [Produces("application/json")]
    [Route("api/applicationusers")]
    [EnableCors("AllowAllOrigins")]
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        public IEnumerable<ApplicationUser> GetApplicationUser()
        {
            return _context.ApplicationUser;
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("{username}")]
        public async Task<IActionResult> GetApplicationUser([FromRoute] string username)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.UserName == username);
            var emergencyContacts = await _context.EmergencyContacts.SingleOrDefaultAsync(c => c.EmergencyContactId == applicationUser.EmergencyContactId);
            applicationUser.EmergencyContacts.Name1 = emergencyContacts.Name1;
            applicationUser.EmergencyContacts.Phone1 = emergencyContacts.Phone1;
            applicationUser.EmergencyContacts.Name2 = emergencyContacts.Name2;
            applicationUser.EmergencyContacts.Phone2 = emergencyContacts.Phone2;

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(applicationUser);
        }

        // PUT: api/ApplicationUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUser([FromRoute] string id, [FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
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

        // POST: api/ApplicationUsers
        [HttpPost]
        public async Task<IActionResult> PostApplicationUser([FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ApplicationUser.Add(applicationUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/ApplicationUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();

            return Ok(applicationUser);
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
