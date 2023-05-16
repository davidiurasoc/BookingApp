using System;
using System.ComponentModel.DataAnnotations;

namespace BookingApp_v2.Data
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
