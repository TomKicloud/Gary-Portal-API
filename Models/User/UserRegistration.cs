using System;
using System.ComponentModel.DataAnnotations;

namespace GaryPortalAPI.Models
{
    public class UserRegistration
    {
        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserFullName { get; set; }

        [Required]
        public string UserPassword { get; set; }
    }
}
