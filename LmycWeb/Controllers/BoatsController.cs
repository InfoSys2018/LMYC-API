﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Authorization;
using LmycWeb.ViewModels;
using System.IO;

namespace LmycWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BoatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Boats
        public async Task<IActionResult> Index()
        {
            return View(await _context.Boats.ToListAsync());
        }

        // GET: Boats/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boats
                .SingleOrDefaultAsync(m => m.BoatId == id);
            if (boat == null)
            {
                return NotFound();
            }

            return View(boat);
        }

        // GET: Boats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoatId,Name,CreditsPerHour,Status,Photo,Description,Length,Make,Year")] BoatViewModel boatViewModel)
        {
            if (ModelState.IsValid)
            {
                var boat = new Boat
                {
                    BoatId = boatViewModel.BoatId,
                    Name = boatViewModel.Name,
                    CreditsPerHour = boatViewModel.CreditsPerHour,
                    Status = boatViewModel.Status,
                    Description = boatViewModel.Description,
                    Length = boatViewModel.Length,
                    Make = boatViewModel.Make,
                    Year = boatViewModel.Year
                };

                if (boatViewModel.Photo == null)
                {
                    string path = Directory.GetCurrentDirectory();

                    boat.Photo = System.IO.File.ReadAllBytes(path + "\\images\\defaultBoat.jpeg");
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await boatViewModel.Photo.CopyToAsync(memoryStream);
                        boat.Photo = memoryStream.ToArray();
                    }
                }

                _context.Add(boat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(boatViewModel);
        }

        // GET: Boats/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boats.SingleOrDefaultAsync(m => m.BoatId == id);

            var boatViewModel = new BoatViewModel
            {
                BoatId = boat.BoatId,
                Name = boat.Name,
                CreditsPerHour = boat.CreditsPerHour,
                Status = boat.Status,
                Description = boat.Description,
                Length = boat.Length,
                Make = boat.Make,
                Year = boat.Year
            };

            if (boat == null)
            {
                return NotFound();
            }
            return View(boatViewModel);
        }

        // POST: Boats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BoatId,Name,CreditsPerHour,Status,Photo,Description,Length,Make,Year")] BoatViewModel boatViewModel)
        {
            if (id != boatViewModel.BoatId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var boat = _context.Boats.Find(id);

                    boat.BoatId = boatViewModel.BoatId;
                    boat.Name = boatViewModel.Name;
                    boat.CreditsPerHour = boatViewModel.CreditsPerHour;
                    boat.Status = boatViewModel.Status;
                    boat.Description = boatViewModel.Description;
                    boat.Length = boatViewModel.Length;
                    boat.Make = boatViewModel.Make;
                    boat.Year = boatViewModel.Year;


                    if (boatViewModel.Photo == null)
                    {
                        
                        if (boat.Photo != null)
                        {
                            boat.Photo = boat.Photo;
                        } else
                        {
                            ViewBag.PhotoError = "Upload Photo Please";
                            return View(boatViewModel);
                        }
                    }


                    if (boatViewModel.Photo != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await boatViewModel.Photo.CopyToAsync(memoryStream);
                            boat.Photo = memoryStream.ToArray();
                        }
                    }

                    
                    _context.Update(boat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatExists(boatViewModel.BoatId))
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

            return View(boatViewModel);
        }

        // GET: Boats/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boats
                .SingleOrDefaultAsync(m => m.BoatId == id);
            if (boat == null)
            {
                return NotFound();
            }

            return View(boat);
        }

        // POST: Boats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var boat = await _context.Boats.SingleOrDefaultAsync(m => m.BoatId == id);
            _context.Boats.Remove(boat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatExists(string id)
        {
            return _context.Boats.Any(e => e.BoatId == id);
        }
    }
}
