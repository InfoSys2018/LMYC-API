using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Identity;
using LmycWeb.ViewModels;

namespace LmycWeb.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ApplicationUser.Include(a => a.EmergencyContacts);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .Include(a => a.EmergencyContacts)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        //// GET: ApplicationUsers/Create
        //public IActionResult Create()
        //{
        //    ViewData["EmergencyContactId"] = new SelectList(_context.EmergencyContacts, "EmergencyContactId", "EmergencyContactId");
        //    return View();
        //}

        //// POST: ApplicationUsers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("FirstName,LastName,MemberStatus,SkipperStatus,Street,City,Province,PostalCode,Country,MobilePhone,HomePhone,WorkPhone,SailingQualifications,Skills,SailingExperience,Credits,EmergencyContactId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(applicationUser);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["EmergencyContacts"] = new SelectList(_context.EmergencyContacts, "EmergencyContacts", "EmergencyContacts", applicationUser.EmergencyContacts.Name1);
        //    return View(applicationUser);
        //}

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            
            UserViewModel userViewModel = new UserViewModel
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName
            };
            ViewBag.Role = new SelectList(_context.Roles, "Name", "Name");
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("FirstName,LastName,MemberStatus,SkipperStatus,Street,City,Province,PostalCode,Country,MobilePhone,HomePhone,WorkPhone,SailingQualifications,Skills,SailingExperience,Credits,EmergencyContact,Id,UserName,Email")]
        public async Task<IActionResult> Edit(string id, UserViewModel userViewModel, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    //var updatedUser = _context.Users.SingleOrDefault(x => x.Id == userViewModel.Id);
                    //await this._userManager.AddToRoleAsync(updatedUser, userViewModel.Role);

                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
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
           
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .Include(a => a.EmergencyContacts)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
