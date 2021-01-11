using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimalShelter.Models.MyValidators;

namespace AnimalShelter.Models
{
    public class ShelterLocation
    {
        public string Name { get; set; }
        public string City { get; set; }
        //[PostalCodeValidator2]
        public string Number { get; set; }
        public string Adress { get; set; }
    }
}