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

    public class Test
    {
        // --- time range 1 ---
        TimeRange timeRange1 = new TimeRange(new DateTime(2011, 2, 22, 14, 0, 0),
                                new DateTime(2011, 2, 22, 18, 0, 0));

        

        TimeBlock timeBlock = new TimeBlock();
        ITimeBlock timeBlock1 = new TimeBlock();


        // > TimeRange1: 22.02.2011 14:00:00 - 18:00:00 | 04:00:00 

        // --- time range 2 ---
        TimeRange timeRange2 = new TimeRange(new DateTime(2011, 2, 22, 15, 0, 0),

        new TimeSpan(2, 0, 0));

        // > TimeRange2: 22.02.2011 15:00:00 - 17:00:00 | 02:00:00
    }
}
