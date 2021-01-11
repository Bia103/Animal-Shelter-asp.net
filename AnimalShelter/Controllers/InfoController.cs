using AnimalShelter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimalShelter.Controllers
{
    public class InfoController : Controller
    {
        // GET: Info
        private ApplicationDbContext db = new ApplicationDbContext();
        //private Info i = new Info();
        public ActionResult Index()
        {
            Info i = new Info();
            if (db.Infos.Count() == 0)
            {
                i.Time = DateTime.Now;
                i.NumberOfAnimals = db.Animals.Count();
                i.NumberOfUsers = db.Users.Count();
                i.Message = "Bine ati venit";
                db.Infos.Add(i);
                db.SaveChanges();
            }
            else
            {
                i = db.Infos
                       .OrderByDescending(p => p.Time)
                       .FirstOrDefault();
            }

            ViewBag.info = i;


            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit()
        {
            Info i = new Info();
            return View(i);
            
           
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(Info infoRequest)
        {
            Info i = new Info();
                    
            i.Time = DateTime.Now;
            i.Message = infoRequest.Message;
            i.NumberOfAnimals = db.Animals.Count();
            i.NumberOfUsers = db.Users.Count();
          
            db.Infos.Add(i);
            db.SaveChanges();
                    
                    
           return RedirectToAction("Index");

        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details()
        {
            List<Info> info = db.Infos.ToList();
            ViewBag.Info = info;
            return View();
        }
    }
}