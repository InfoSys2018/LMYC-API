using LmycWeb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Data
{
    public class DBInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        {
            context.Database.EnsureCreated();

            Debug.WriteLine("In Initializer");

            //Create admin role if not found.
            if (await _roleManager.FindByNameAsync("Admin") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //Create full member - good standing role if not found.
            if (await _roleManager.FindByNameAsync("Good Standing Member") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Good Standing Member"));
            }
            //Create full member - not good standing role if not found
            if (await _roleManager.FindByNameAsync("Not Good Standing Member") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Not Good Standing Member"));
            }
            //Create associate member standing role if not found
            if (await _roleManager.FindByNameAsync("Associate Member") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Associate Member"));
            }
            //Create social member standing role if not found
            if (await _roleManager.FindByNameAsync("Social Member") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Social Member"));
            }
            //Create booking moderator standing role if not found
            if (await _roleManager.FindByNameAsync("Booking Moderator") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Booking Moderator"));
            }

            //First member.
            if (await _userManager.FindByEmailAsync("m1@m.m") == null)
            {
                var member1 = new ApplicationUser
                {
                    Email = "m1@m.m",
                    UserName = "m1",
                    FirstName = "Rob",
                    LastName = "Stan",
                    MemberStatus = "Full member good standing",
                    SkipperStatus = "Crew",
                    Street = "Cotton Drive",
                    City = "Vancouver",
                    Province = "British Columbia",
                    PostalCode = "V5N 1A7",
                    Country = "Canada",
                    MobilePhone = "7787770088",
                    HomePhone = "",
                    WorkPhone = "",
                    SailingQualifications = "None",
                    SailingExperience = "None",
                    Credits = 320,
                };
                var result = await _userManager.CreateAsync(member1, "P@$$w0rd");
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(member1, "Good Standing Member");
            }

            if (await _userManager.FindByEmailAsync("a1@a.a") == null)
            {
                var member1 = new ApplicationUser
                {
                    Email = "a1@a.a",
                    UserName = "a1",
                    FirstName = "Derek",
                    LastName = "Livington",
                    MemberStatus = "Full member good standing",
                    SkipperStatus = "Day skipper",
                    Street = "Fraser Street",
                    City = "Vancouver",
                    Province = "British Columbia",
                    PostalCode = "V1D 3B7",
                    Country = "Canada",
                    MobilePhone = "7781234657",
                    HomePhone = "",
                    WorkPhone = "",
                    SailingQualifications = "Sailing Legend",
                    SailingExperience = "Born in the ocean",
                    Credits = 320,
                };
                var result = await _userManager.CreateAsync(member1, "P@$$w0rd");
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(member1, "Admin");
            }

            if (await _userManager.FindByEmailAsync("m2@m.m") == null)
            {
                var member1 = new ApplicationUser
                {
                    Email = "m2@m.m",
                    UserName = "m2",
                    FirstName = "Ryan",
                    LastName = "Smith",
                    MemberStatus = "Full member not good standing",
                    SkipperStatus = "Cruise skipper",
                    Street = "East 11th Avenue",
                    City = "Vancouver",
                    Province = "British Columbia",
                    PostalCode = "V1D 3B7",
                    Country = "Canada",
                    MobilePhone = "6048976544",
                    HomePhone = "",
                    WorkPhone = "",
                    SailingQualifications = "None",
                    SailingExperience = "None",
                    Credits = 0,
                };
                var result = await _userManager.CreateAsync(member1, "P@$$w0rd");
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(member1, "Admin");
            }

            // Seeding boats if there is no record
            if (!context.Boats.Any())
            {
                context.Boats.AddRange(DummyData.GetBoats());
                context.SaveChanges();
            }

            if (!context.Bookings.Any())
            {
                context.Bookings.AddRange(DummyData.GetBookings(context));
                context.SaveChanges();
            }

            if (!context.Members.Any())
            {
                context.Members.AddRange(DummyData.GetMembers(context));
                context.SaveChanges();
            }

            if (!context.NonMembers.Any())
            {
                context.NonMembers.AddRange(DummyData.GetNonMembers(context));
                context.SaveChanges();
            }

            if (!context.Reports.Any())
            {
                context.Reports.AddRange(DummyData.GetReports(context));
                await context.SaveChangesAsync();
            }

            context.SaveChanges();

        }
    }
}
