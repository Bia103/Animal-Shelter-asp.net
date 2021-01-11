using Microsoft.SqlServer.Server;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnimalShelter.Models
{
    public class Shelter
    {
        [Key]
        public int ShelterId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        [Required]
        public virtual Location Location { get; set; }
    }
}