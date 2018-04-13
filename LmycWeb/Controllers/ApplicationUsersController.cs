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

            var users = _context.ApplicationUser.Include(a => a.EmergencyContacts);

            foreach (var user in users)
            {
                user.TotalHours = _context.Reports.Where(r => r.UserId == user.Id).Sum(r => r.Hours);
            }

            return View(await users.ToListAsync());
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
        public async Task<IActionResult> Edit(string id, ApplicationUser applicationUser)
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

                    var user = _context.ApplicationUser.Find(id);

                    user.UserName = applicationUser.UserName;
                    user.Email = applicationUser.Email;
                    user.FirstName = applicationUser.FirstName;
                    user.LastName = applicationUser.LastName;
                    user.MemberStatus = applicationUser.MemberStatus;
                    user.SkipperStatus = applicationUser.SkipperStatus;
                    user.Street = applicationUser.Street;
                    user.Province = applicationUser.Province;
                    user.Country = applicationUser.Country;
                    user.MobilePhone = applicationUser.MobilePhone;
                    user.WorkPhone = applicationUser.WorkPhone;
                    user.SailingQualifications = applicationUser.SailingQualifications;
                    user.Skills = applicationUser.Skills;
                    user.SailingExperience = applicationUser.SailingExperience;
                    user.Credits = applicationUser.Credits;
                    user.EmergencyContacts = applicationUser.EmergencyContacts;

                    _context.Update(user);

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

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
