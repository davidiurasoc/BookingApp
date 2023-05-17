using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApp_v2.Data
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string RoomTypeName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
