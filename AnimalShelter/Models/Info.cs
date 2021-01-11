using AnimalShelter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalShelter.Models
{
    public class Info
    {
        [Key]
        public int InfoId { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfAnimals { get; set; }
    }
}