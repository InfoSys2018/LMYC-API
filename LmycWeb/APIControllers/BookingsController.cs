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
    [Route("api/Bookings")]
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [EnableCors("AllowAllOrigins")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public IEnumerable<Booking> GetBookings()
        {
            return _context.Bookings;
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }


        // GET: api/Bookings/5
        [Route("boat/{id}")]
        [HttpGet]
        public IActionResult GetBookingByBoat([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = _context.Bookings.Where(m => m.BoatId == id).Where(m => m.StartDateTime > DateTime.Now);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }


        // GET: api/Bookings/5
        [Route("user/{id}")]
        [HttpGet]
        public IActionResult GetBookingByUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = _context.Bookings.Where(m => m.UserId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }



        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking([FromRoute] string id, [FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            //Check if the members have enough for the newly allocated credits
            if (booking.CreditsUsed != 0)
            {
                bool result = await CheckMembersHaveEnoughCreditsForEditAsync(booking.Members, id);

                if (!result)
                {
                    return BadRequest("A member does not have enough credits.");
                }
            }

            //Refund old charges then Charge the newly allocated credits to each user 
            //if there is any credits to be charged
            if (booking.CreditsUsed != 0)
            {
                var oldBooking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == id);
                RefundBookingMemberCredits(oldBooking.Members);
                ChargeBookingMemberCredits(booking.Members);
            }

            _context.Entry(booking).State = EntityState.Modified;

            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        [HttpPost]
        public async Task<IActionResult> PostBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check the member status of the user creating the booking
            bool goodStandingResult = await FullMemberGoodStatusCheckAsync(booking.UserId);

            if (!goodStandingResult)
            {
                return BadRequest("The user can't create the booking because they are not in good standing.");
            }

            //Check if the booking requires credits
            if (booking.CreditsUsed != 0)
            {
                bool result = await CheckMembersHaveEnoughCreditsAsync(booking.Members);

                if (!result)
                {
                    return BadRequest("A member does not have enough credits");
                }
            }

            int totalDays = (booking.EndDateTime - booking.StartDateTime).Days;

            bool skipperStatusResult;

            if (totalDays >= 1)
            {
                skipperStatusResult = await CheckSkipperStatusForOverNightAsync(booking.Members);
                if (!skipperStatusResult)
                {
                    return BadRequest("One of the members must have a Skipper Status of Cruise");
                }
            }
            else
            {
                skipperStatusResult = await CheckSkipperStatusForDayAsync(booking.Members);
                if (!skipperStatusResult)
                {
                    return BadRequest("One of the members must have a Skipper Status of Day");
                }
            }


            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            //Charge the credits to each user if there are any credits to charge
            if (booking.CreditsUsed != 0)
            {
                ChargeBookingMemberCredits(booking.Members);
            }

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            RefundBookingMemberCredits(booking.Members);

            return Ok(booking);
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