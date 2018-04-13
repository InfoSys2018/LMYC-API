using LmycWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LmycWeb.Interfaces
{
        public interface IDbContext
    {
            DbSet<Boat> Boats { get; set; }
            DbSet<Document> Documents { get; set; }
            DbSet<Report> Reports { get; set; }
            DbSet<ClassificationCode> ClassificationCodes { get; set; }
            DbSet<EmergencyContact> EmergencyContacts { get; set; }
            DbSet<Booking> Bookings { get; set; }
            DbSet<Member> Members { get; set; }
            DbSet<NonMember> NonMembers { get; set; }
            DbSet<LmycWeb.Models.ApplicationUser> ApplicationUser { get; set; }
            DbSet<Contact> Contacts { get; set; }

        int SaveChanges();
            Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
            EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        }
    
}
