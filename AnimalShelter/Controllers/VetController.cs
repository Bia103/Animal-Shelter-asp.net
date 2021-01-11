using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimalShelter.Models;

namespace AnimalShelter.Controllers
{
    public class VetController : Controller
    {
        // GET: Vet
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<Vet> vets = db.Vets.ToList();
            ViewBag.Vets = vets;
            return View();
        }

        
        [HttpGet]
        public ActionResult New()
        {
            Vet a = new Vet();
            return View(a);
        }

        [HttpPost]
        public ActionResult New(Vet vetRequest)
        {

            try
            {
               
                if (ModelState.IsValid)
                {

                    db.Vets.Add(vetRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(vetRequest);
            }
            catch (Exception e)
            {
                return View(vetRequest);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Vet v = db.Vets.Find(id);
                if (v == null)
                {
                    return HttpNotFound("Couldn't find the vet with id " + id.ToString());
                }
                
                return View(v);
            }
            return HttpNotFound("Missing book id parameter!");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Vet vetRequest)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    Vet v = db.Vets.SingleOrDefault(b => b.VetId.Equals(id));
                    if (TryUpdateModel(v))
                    {
                        v.Name = vetRequest.Name;
                        v.PhoneNumber = vetRequest.PhoneNumber;
                        
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(vetRequest);
            }
            catch (Exception e)
            {
                return View(vetRequest);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Vet a = db.Vets.Find(id);
            if (a != null)
            {
                db.Vets.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the vet with id " + id.ToString());
        }
    }
}