using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingApp_v2.Models
{
    public class ClientVM
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        public string TaxId { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Date Joined")]
        public DateTime DateJoined { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
