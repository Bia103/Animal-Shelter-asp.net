using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalShelter.Models
{
    public class UserViewModel
    {
        public ApplicationUser User{ get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
    }
}