using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BookingApp_v2.Models;

namespace BookingApp_v2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RoomBooking> RoomBookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BookingApp_v2.Models.ClientVM> ClientVM { get; set; }
    }
}
