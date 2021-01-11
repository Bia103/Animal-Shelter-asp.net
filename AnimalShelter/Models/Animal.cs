using Microsoft.SqlServer.Server;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace AnimalShelter.Models
{
    public class Animal
    {
        [Key]
        public int AnimalId { get; set; }
        [RegularExpression(@"^[^0-9]+$", ErrorMessage = "This is not a valid name! It should not contain numbers.")]
        public string Name { get; set; }
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "This is not a valid age! It should contain only numbers.")]
        public string Age { get; set; }
        [RegularExpression(@"^[^0-9]+$", ErrorMessage = "This is not a valid name! It should not contain numbers.")]
        public string Race { get; set; }
        
        [ForeignKey("Shelter")]
        public int ShelterId { get; set; }
        public virtual Shelter Shelter { get; set; }
        //[NotMapped]
        public IEnumerable<SelectListItem> ShelterList { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
        
    }
  /*  public class DbCtx : DbContext
    {
        public DbCtx() : base("DbConnectionString")
        {
            Database.SetInitializer<DbCtx>(new Initp());
            //Database.SetInitializer<DbCtx>(new CreateDatabaseIfNotExists<DbCtx>());
            //Database.SetInitializer<DbCtx>(new DropCreateDatabaseIfModelChanges<DbCtx>());
            //Database.SetInitializer<DbCtx>(new DropCreateDatabaseAlways<DbCtx>());
        }
        public DbSet<Animal> Animals { get; set; }

    }
    public class Initp : DropCreateDatabaseAlways<DbCtx>
    {
        protected override void Seed(DbCtx ctx)
        {
            ctx.Animals.Add(new Animal
            {
                Name = "Bobocel",
                Age = "3",
                ShelterId = "4",
                Race = "Cat"
            });
            ctx.Animals.Add(new Animal
            {
                Name = "Bumbacel",
                Age = "7",
                ShelterId = "1",
                Race = "Dog"
            });
            ctx.SaveChanges();
            base.Seed(ctx);
        }
    }*/
}