using AnimalShelter.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace AnimalShelter.Controllers
{
    
    public class UsersController : Controller
    {
        // GET: Users
        
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [NonAction]
        public List<CheckBoxViewModel> GetAllAnimals()
        {
            var checkboxList = new List<CheckBoxViewModel>();
            foreach (var a in ctx.Animals.ToList())
            {
                checkboxList.Add(new CheckBoxViewModel
                {
                    Id = a.AnimalId,
                    Name = a.Name,
                    Checked = false
                });
            }
            return checkboxList;
        }
        
        [HttpGet]
        public ActionResult TakeCare()
        {
            
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = ctx.Users.Find(id);
            user.AnimalList = GetAllAnimals();
            foreach (Animal checkedAnimal in user.Animals)
            { // iteram prin genurile care erau atribuite cartii inainte de momentul accesarii formularului
                // si le selectam/bifam in lista de checkbox-uri
                user.AnimalList.FirstOrDefault(g => g.Id == checkedAnimal.AnimalId).Checked = true;
            }
            List<Animal> animals = ctx.Animals.ToList();
            ViewBag.Animals = animals;
            
            return View(user);

        }
        [HttpPut]
        public ActionResult TakeCare(string id2)
        {
            

            // preluam cartea pe care vrem sa o modificam din baza de date
            ///Book book = db.Books.Include("Publisher").Include("BookType").SingleOrDefault(b => b.BookId.Equals(id));
            // memoram intr-o lista doar genurile care au fost selectate din formular
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = ctx.Users.Find(id);
            Animal selectedAnimal = ctx.Animals.Find(id2);
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(user))
                    {


                           user.Animals.Add(selectedAnimal);
                        
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index", "Info");
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPut]
        public ActionResult TakeCare(ApplicationUser userRequest)
        {
            userRequest.AnimalList = GetAllAnimals();

            // preluam cartea pe care vrem sa o modificam din baza de date
            ///Book book = db.Books.Include("Publisher").Include("BookType").SingleOrDefault(b => b.BookId.Equals(id));
            // memoram intr-o lista doar genurile care au fost selectate din formular
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = ctx.Users.Find(id);
            var selectedAnimals = userRequest.AnimalList.Where(b => b.Checked).ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(user))
                    {

                        user.Animals.Clear();
                        user.Animals = new List<Animal>();
                        for (int i = 0; i < selectedAnimals.Count(); i++)
                        {
                            // cartii pe care vrem sa o editam ii asignam genurile selectate
                            Animal a = ctx.Animals.Find(selectedAnimals[i].Id);
                            user.Animals.Add(a);
                        }
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Index", "Info");
                }
                return View(userRequest);
            }
            catch (Exception)
            {
                return View(userRequest);
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.UsersList = ctx.Users.Include("Animals").ToList();
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return HttpNotFound("Missing the id parameter!");
            }

            ApplicationUser user = ctx.Users
            .Include("Roles")
            .FirstOrDefault(u => u.Id.Equals(id));

            if (user != null)
            {
                ViewBag.UserRole = ctx.Roles
                .Find(user.Roles.First().RoleId).Name;
                ViewBag.UserId = id;
                return View(user);
            }
            return HttpNotFound("Cloudn't find the user with given id!");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return HttpNotFound("Missing the id parameter!");
            }

            UserViewModel uvm = new UserViewModel();
            uvm.User = ctx.Users.Find(id);

            IdentityRole userRole = ctx.Roles.Find(uvm.User.Roles.First().RoleId);
            uvm.RoleName = userRole.Name;
            return View(uvm);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(string id, UserViewModel uvm)
        {
            ApplicationUser user = ctx.Users.Find(id);
            try
            {
                if (TryUpdateModel(user))
                {
                    var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));

                    foreach (var r in ctx.Roles.ToList())
                    {
                        um.RemoveFromRole(user.Id, r.Name);
                    }
                    um.AddToRole(user.Id, uvm.RoleName);

                    user.UserName = uvm.User.Email;
                    user.Email = uvm.User.Email;
                    ctx.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(uvm);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
         public ActionResult Delete(string id)
         {
             ApplicationUser user = ctx.Users.Find(id);
             if (user != null)
             {
                 ctx.Users.Remove(user);
                 ctx.SaveChanges();
                 return RedirectToAction("Index");
             }
            return HttpNotFound("Couldn't find the user with id " + id.ToString() + " !");
         }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            UserViewModel user = new UserViewModel();

            return View(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(UserViewModel userRequest)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
                    //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
                    
                    //System.Diagnostics.Debug.WriteLine("da");
                    var user = new ApplicationUser
                    {
                        UserName = userRequest.User.UserName,
                        Email = userRequest.User.Email
                    };
                    //var result = await UserManager.CreateAsync(user, userRequest.Password);
                    var result =  UserManager.Create(user, userRequest.Password);
                    if (result.Succeeded)
                    {
                        SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                        var roleManager = new RoleManager<IdentityRole>(roleStore);

                        if (!roleManager.RoleExists("User"))
                            roleManager.Create(new IdentityRole("User"));
                        UserManager.AddToRole(user.Id, "User");
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View(userRequest);
            }
            catch (Exception e)
            {
                return View(userRequest);
            }
        }

        /*public ActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return HttpNotFound("Missing the id parameter!");
            }

            UserViewModel uvm = new UserViewModel();
            uvm.User = ctx.Users.Find(id);

            IdentityRole userRole = ctx.Roles.Find(uvm.User.Roles.First().RoleId);
            uvm.RoleName = userRole.Name;
            return View(uvm);
        }
        [HttpPost]
        public ActionResult Delete(string id, UserViewModel uvm)
        {

        }*/

    }
}