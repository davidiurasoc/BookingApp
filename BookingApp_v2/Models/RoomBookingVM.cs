using BookingApp_v2.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingApp_v2.Models
{
    public class RoomBookingVM
    {
        public int Id { get; set; }

        public ClientVM BookingClient { get; set; }
        [Display(Name = "Client Name")]
        public string BookingClientId { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        public string StartDate { get; set; }
        
        [Display(Name = "Start Date")]
        [Required]
        public string EndDate { get; set; }
        
        public IEnumerable<SelectListItem> RoomTypes { get; set; }
        
        public RoomVM RoomType { get; set; }
        [Display(Name = "Room Type")]
        public int RoomTypeId { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }

        public List<RoomBookingVM> RoomBookings { get; set; }

    }

    public class AdminRoomBookingsViewVM
    {
        [Display(Name = "Total Number Of Bookings")]
        public int TotalBookings { get; set; }
        public List<RoomBookingVM> RoomBookings { get; set; }
    }

    public class CreateRoomBookingVM
    {
        [Display(Name = "Start Date")]
        [Required]
        public string StartDate { get; set; }
        [Display(Name = "Start Date")]
        [Required]
        public string EndDate { get; set; }
        public IEnumerable<SelectListItem> RoomTypes { get; set; }
        [Display(Name = "Start Date")]
        public int RoomTypeId { get; set; }

    }

    public class ClientRoomBookingViewVM
    {
        public List<RoomBookingVM> RoomBookings { get; set; }
    }
}
