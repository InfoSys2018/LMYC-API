using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LmycWeb.Models;
using LmycWeb.Interfaces;

namespace LmycWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Member>()
                .HasKey(m => new { m.UserId, m.BookingId });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Boat> Boats { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ClassificationCode> ClassificationCodes { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<NonMember> NonMembers { get; set; }
        public DbSet<LmycWeb.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
