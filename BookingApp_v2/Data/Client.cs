using Itenso.TimePeriod;
using Microsoft.AspNetCore.Identity;
using System;

namespace BookingApp_v2.Data
{
    public class Client : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TaxId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
