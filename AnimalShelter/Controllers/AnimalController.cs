using AnimalShelter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimalShelter.Controllers
{
    public class AnimalController : Controller
    {
        // GET: Animal
        private ApplicationDbContext db = new ApplicationDbContext();
        [NonAction] // specificam faptul ca nu este o actiune
        public IEnumerable<SelectListItem> GetAllShelters()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            foreach (var s in db.Shelters.ToList())
            {
                // adaugam in lista elementele necesare pt dropdown
                selectList.Add(new SelectListItem
                {
                    Value = s.ShelterId.ToString(),
                    Text = s.Name
                });
            }
            // returnam lista pentru dropdown
            return selectList;
        }

        public ActionResult IndexForUser()
        {
            List<Animal> animals = db.Animals.ToList();
            ViewBag.Animals = animals;
            return View();
        }
        
        [Authorize(Roles = "Admin,Voluntar")]
        public ActionResult Index()
        {
            List<Animal> animals = db.Animals.ToList();
            ViewBag.Animals = animals;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New()
        {
            Animal a = new Animal();
            a.ShelterList = GetAllShelters();
            //ViewBag.numnerOfShelters
            return View(a);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(Animal animalRequest)
        {
            
            try
            {
                animalRequest.ShelterList = GetAllShelters();
                if (ModelState.IsValid)
                {
                    Shelter s = db.Shelters.Find(animalRequest.ShelterId);
                    animalRequest.Shelter = s;
                    db.Animals.Add(animalRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(animalRequest);
            }
            catch (Exception e)
            {
                return View(animalRequest);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Animal animal = db.Animals.Find(id);
                if (animal == null)
                {
                    return HttpNotFound("Couldn't find the book with id " + id.ToString());
                }
                animal.ShelterList = GetAllShelters();
                return View(animal);
            }
            return HttpNotFound("Missing book id parameter!");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id,Animal animalRequest)
        {
            try
            {
                animalRequest.ShelterList = GetAllShelters();
                if (ModelState.IsValid)
                {
                    Animal animal= db.Animals.SingleOrDefault(b => b.AnimalId.Equals(id));
                    if (TryUpdateModel(animal))
                    {
                        animal.Name = animalRequest.Name;
                        animal.Age = animalRequest.Age;
                        animal.Race = animalRequest.Race;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(animalRequest);
            }
            catch (Exception e)
            {
                return View(animalRequest);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Animal a = db.Animals.Find(id);
            if (a!= null)
            {
                db.Animals.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the book with id " + id.ToString());
        }



    }
}