using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp_v2.Data
{
    public class RoomBooking
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("BookingClientId")]
        public Client BookingClient { get; set; }
        public string BookingClientId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("RoomTypeId")]
        public RoomType RoomType { get; set; }
        public int RoomTypeId { get; set; }

        public DateTime DateRequested { get; set; }
    }
}
