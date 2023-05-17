using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace BookingApp_v2.Models
{
    public class RoomVM
    {
        public int Id { get; set; }
        [Required]
        public string RoomName { get; set; }
        public DateTime DateCreated { get; set; }

        public List<RoomBookingVM> RoomBookings { get; set; }
    }
}
