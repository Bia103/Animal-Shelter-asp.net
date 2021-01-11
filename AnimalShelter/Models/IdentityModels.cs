using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalShelter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<Animal> Animals { get; set; }
        [NotMapped]
        public List<CheckBoxViewModel> AnimalList { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Info> Infos { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class Initp : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext ctx)
        {
            Location l1 = new Location { LocationId = 1, City = "Bucharest", Adress = "Piata Victoriei", Number = "517006" };
            Location l2 = new Location { LocationId = 2, City = "Ploiesti", Adress = "Bulevardul Republicii", Number = "517007" };

            Shelter shelter1 = new Shelter { ShelterId = 1, Name = "AnimalSanctuary", Location = l1 };
            Shelter shelter2 = new Shelter { ShelterId = 2, Name = "Ani", Location = l2 };

            l1.Shelter = shelter1;
            l2.Shelter = shelter2;

            ctx.Shelters.Add(shelter1);
            ctx.Shelters.Add(shelter2);

            ctx.Locations.Add(l1);
            ctx.Locations.Add(l2);

            ctx.Animals.Add(new Animal
            {
                Name = "Bobocel",
                Age = "3",
                Race = "Cat",
                Shelter = shelter1


            });
            ctx.Animals.Add(new Animal
            {
                Name = "Bumbacel",
                Age = "7",
         
                Race = "Dog",
                Shelter = shelter2
            });

            Vet vet1 = new Vet { VetId = 1, Name = "Cosmin",  PhoneNumber = "0765345123" };
            Vet vet2 = new Vet { VetId = 2, Name = "Cosmina", PhoneNumber = "0765344123" };

            ctx.Vets.Add(vet1);
            ctx.Vets.Add(vet2);

            ctx.SaveChanges();
            base.Seed(ctx);
        }
    }
}