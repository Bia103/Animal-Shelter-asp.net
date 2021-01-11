using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimalShelter.Models;

namespace AnimalShelter.Controllers
{
    public class ShelterController : Controller
    {
        // GET: Shelter
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Shelters = db.Shelters.ToList();
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Shelter s = db.Shelters.Find(id);
                if (s != null)
                {
                    //ViewBag.Region = ctx.Regions.Find(publisher.ContactInfo.RegionId).Name;
                    return View(s);
                }
                return HttpNotFound("Couldn't find the publisher with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing publisher id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New()
        {
            ShelterLocation sl = new ShelterLocation();
            return View(sl);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(ShelterLocation slRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Location l = new Location
                    {
                        City = slRequest.City,
                        Number = slRequest.Number,
                        Adress = slRequest.Adress
                    };
                    // vom adauga in baza de date ambele obiecte
                    
                    Shelter s = new Shelter
                    {
                        Name = slRequest.Name,
                        Location = l
                    };
                    l.Shelter = s;
                    db.Shelters.Add(s);
                    db.Locations.Add(l);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(slRequest);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(slRequest);
            }

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Shelter s = db.Shelters.Find(id);
                if (s == null)
                {
                    return HttpNotFound("Couldn't find the shelter with id " + id.ToString());
                }
                
                return View(s);
            }
            return HttpNotFound("Missing book id parameter!");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Shelter shelterRequest)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    Shelter s = db.Shelters.SingleOrDefault(b => b.ShelterId.Equals(id));
                    if (TryUpdateModel(s))
                    {
                        s.Name = shelterRequest.Name;
                        s.Location.City = shelterRequest.Location.City;
                        s.Location.Adress = shelterRequest.Location.Adress;
                        s.Location.Number = shelterRequest.Location.Number;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(shelterRequest);
            }
            catch (Exception e)
            {
                return View(shelterRequest);
            }
        }
        public ActionResult Delete(int id)
        {
            Shelter a = db.Shelters.Find(id);
            if (a != null)
            {
                db.Shelters.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the book with id " + id.ToString());
        }
    }
}