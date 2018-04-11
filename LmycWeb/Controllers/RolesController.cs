using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmycWeb.Data;
using LmycWeb.Models;
using LmycWeb.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmycWeb.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Roles
        public async Task<ActionResult> Index()
        {
            var roles = _roleManager.Roles;
            return View(await roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityRole = await _roleManager.FindByIdAsync(id);
            if (identityRole == null)
            {
                return NotFound();
            }

            var users = _userManager.GetUsersInRoleAsync(identityRole.Id).Result;

            var role = new RoleViewModel
            {
                RoleId = identityRole.Id,
                RoleName = identityRole.Name,
                Users = users
            };
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityRole identityRole)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    if (await _roleManager.RoleExistsAsync(identityRole.Name))
                    {
                        return View();
                    }
                }
                await _roleManager.CreateAsync(identityRole);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityRole = await _roleManager.FindByIdAsync(id);
            if (identityRole == null)
            {
                return NotFound();
            }

            var usersRole = _userManager.GetUsersInRoleAsync(identityRole.Name).Result.ToList();

            var role = new RoleViewModel
            {
                RoleId = identityRole.Id,
                RoleName = identityRole.Name,
                Users = usersRole
            };
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityRole = await _roleManager.FindByIdAsync(id);

            if (identityRole == null || identityRole.Name.Equals("Admin"))
            {
                return NotFound();
            }

            var users = _userManager.GetUsersInRoleAsync(identityRole.Name).Result;

            var role = new RoleViewModel
            {
                RoleId = identityRole.Id,
                RoleName = identityRole.Name,
                Users = users
            };

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                // TODO: Add delete logic here

                var role = await _roleManager.FindByIdAsync(id);

                if (role.Name.Equals("Admin") || role == null)
                {
                    return NotFound();
                }

                var result = await _roleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Roles/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(RoleViewModel roleModel)
        {
            var user = await _userManager.FindByIdAsync(roleModel.UserId);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.RemoveFromRoleAsync(user, roleModel.RoleName);

            return RedirectToAction(nameof(Details), new { id = roleModel.RoleId });
        }
    }
}