using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimalShelter.Models.MyValidators;

namespace AnimalShelter.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string City { get; set; }
        [PostalCodeValidator]
        public string Number { get; set; }
        public string Adress { get; set; }
        public virtual Shelter Shelter { get; set; }
    }
}