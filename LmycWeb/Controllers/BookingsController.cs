using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace LmycWeb.Controllers
{
    [Authorize (Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.Boat).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Boat)
                .Include(b => b.User)
                .SingleOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["BoatId"] = new SelectList(_context.Boats, "BoatId", "BoatId");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,StartDateTime,EndDateTime,CreditsUsed,BoatId,Itinerary,UserId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoatId"] = new SelectList(_context.Boats, "BoatId", "BoatId", booking.BoatId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["BoatId"] = new SelectList(_context.Boats, "BoatId", "BoatId", booking.BoatId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookingId,StartDateTime,EndDateTime,CreditsUsed,BoatId,Itinerary,UserId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["BoatId"] = new SelectList(_context.Boats, "BoatId", "BoatId", booking.BoatId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Boat)
                .Include(b => b.User)
                .SingleOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var booking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(string id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }




        //*************************** HELPER FUNCTIONS *********************************

        public async Task<bool> FullMemberGoodStatusCheckAsync(string userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == userId);

            if (user.MemberStatus.Equals("full member good standing", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CheckMembersHaveEnoughCreditsAsync(List<Member> members)
        {
            foreach (var member in members)
            {
                var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == member.UserId);
                if (user.Credits < member.AllocatedCredits)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> CheckMembersHaveEnoughCreditsForEditAsync(List<Member> members, string bookingId)
        {
            //Grab the old booking and its members from the context
            var oldBooking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == bookingId);
            List<Member> oldMembers = oldBooking.Members;

            foreach (var member in members)
            {
                //Grab the member user
                var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == member.UserId);

                //Grab the oldmember if one exists 
                var oldMember = oldMembers.SingleOrDefault(m => m.UserId == user.Id);

                //If there is no old member then check that they have enough credits
                if (oldMember == null)
                {
                    if (user.Credits < member.AllocatedCredits)
                    {
                        return false;
                    }

                }
                //If there is an old member, add their previously charged credits to their 
                //current credit and check if they have enough credits for the new allocation
                else
                {
                    if ((user.Credits + oldMember.AllocatedCredits) < member.AllocatedCredits)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task RefundAndChargeNewAllocationAsync(List<Member> members, string bookingId)
        {
            //Grab the old booking and its members from the context
            var oldBooking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == bookingId);
            List<Member> oldMembers = oldBooking.Members;

            foreach (var member in members)
            {
                //Grab the member user
                var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == member.UserId);

                //Grab the oldmember if one exists 
                var oldMember = oldMembers.SingleOrDefault(m => m.UserId == user.Id);

                //If there is no old member charge them the credits
                if (oldMember == null)
                {
                    user.Credits = user.Credits - member.AllocatedCredits;
                }
                //If there is an old member, refund their previously charged credits to their 
                //current credit and charge them the new amount
                else
                {
                    user.Credits = user.Credits + oldMember.AllocatedCredits - member.AllocatedCredits;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async void ChargeBookingMemberCredits(List<Member> members)
        {
            foreach (var member in members)
            {
                var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == member.UserId);
                user.Credits = user.Credits - member.AllocatedCredits;
            }
            await _context.SaveChangesAsync();
        }

        public async void RefundBookingMemberCredits(List<Member> members)
        {
            foreach (var member in members)
            {
                var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == member.UserId);
                user.Credits = user.Credits + member.AllocatedCredits;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckSkipperStatusForOverNightAsync(List<Member> members)
        {
            foreach (var member in members)
            {
                var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == member.UserId);
                if (user.SkipperStatus.Equals("cruise skipper", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CheckSkipperStatusForDayAsync(List<Member> members)
        {
            foreach (var member in members)
            {
                var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == member.UserId);
                if (user.SkipperStatus.Equals("day skipper", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
