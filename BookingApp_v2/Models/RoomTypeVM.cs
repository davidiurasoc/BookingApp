using System.ComponentModel.DataAnnotations;
using System;

namespace BookingApp_v2.Models
{
    public class RoomTypeVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
