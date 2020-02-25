using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlackBelt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;



namespace BlackBelt.Controllers
{
    public class HomeController : Controller
    {
        private Context dbContext;
        public HomeController(Context context)
        {
            dbContext = context;
        }

    // HANDLING LOGIN/REGISTRATION FORM ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

    // HANDLING LOGOUT ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }


    // HANDLING HOME PAGE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("home")]
        public IActionResult Home()
        {
            int? Session = HttpContext.Session.GetInt32("UserInSession");
            if(Session == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CurrentUser = dbContext.User
                .FirstOrDefault(y => y.UserId == HttpContext.Session.GetInt32("UserInSession"));

            List<Actividad> AllActividad = dbContext.Actividad
                .Include(z => z.Participants)
                .ThenInclude(y => y.User)
                .OrderBy(a => a.Date)
                .ThenBy(b => b.Tiempo)
                .ToList();
            ViewBag.AllUsers = dbContext.User
                .ToList();

            return View(AllActividad);
        }

    // HANDLING NEW ACTIVITY PAGE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("newactivity")]
        public IActionResult NewActivity()
        {
            int? Session = HttpContext.Session.GetInt32("UserInSession");
            if(Session == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CurrentUser = dbContext.User
                .FirstOrDefault(y => y.UserId == HttpContext.Session.GetInt32("UserInSession"));

            

            return View();
        }

        [HttpGet("actividadinfo/{IdActividad}")]
        public IActionResult Info(int IdActividad)
        {
            int? Session = HttpContext.Session.GetInt32("UserInSession");
            if(Session == null)
            {
                return RedirectToAction("Index");
            }
            Actividad THIS = dbContext.Actividad
                .Include(y => y.Participants)
                .ThenInclude(x => x.User)
                .FirstOrDefault(z => z.ActividadId == IdActividad);
            ViewBag.AllUsers = dbContext.User
                .ToList();
            ViewBag.CurrentUser = dbContext.User
                .FirstOrDefault(y => y.UserId == HttpContext.Session.GetInt32("UserInSession"));
            return View(THIS);
        }


// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~POST~~~~~REQUESTS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



     // HANDLING REGISTRATION ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("register")]
        public IActionResult register(User NewUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.User.Any(u => u.Email == NewUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);

                dbContext.Add(NewUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserInSession", NewUser.UserId);
                return RedirectToAction("Home");
            }
            return View("Index");
        }

     // HANDLING LOGIN ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("login")]
        public IActionResult login(LoginUser AccessUser)
        {
            if(ModelState.IsValid)
            {
                var UserInDb = dbContext.User.FirstOrDefault(u => u.Email == AccessUser.LEmail); 
                if(UserInDb == null)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(AccessUser, UserInDb.Password, AccessUser.LPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserInSession", UserInDb.UserId);
                return RedirectToAction("Home");
            }
            return View("Index");
        }

     // HANDLING NEW ACTIVITY ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("newactividad")]
        public IActionResult newactividad(Actividad NewActividad)
        {
            if(ModelState.IsValid)
            {
                if(NewActividad.Date < DateTime.Today)
                {
                    ModelState.AddModelError("Date", "Activity cannot be in the past.");
                    return View("NewActivity");
                }
                User CurrentUser = dbContext.User
                .FirstOrDefault(y => y.UserId == HttpContext.Session.GetInt32("UserInSession"));

                NewActividad.UserId = (int)HttpContext.Session.GetInt32("UserInSession");
                dbContext.Add(NewActividad);
                dbContext.SaveChanges();
                
                return RedirectToAction("Home");
            }
            return View("NewActivity");
        }
    
     // HANDLING JOIN ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
        [HttpPost("join/{IdAct}")]
        public IActionResult join(UserActividadRelation NewRelation, int IdAct)
        {
            NewRelation.ActividadId = IdAct;
            NewRelation.UserId = (int)HttpContext.Session.GetInt32("UserInSession");

            dbContext.Add(NewRelation);
            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }
    // // HANDLING LEAVE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("leave/{IdAct}")]
        public IActionResult dontgotowedding(int IdAct)
        {
            int session = (int)HttpContext.Session.GetInt32("UserInSession");

            UserActividadRelation RelationToDelete = dbContext.UserActividadRelation.Where(m => m.ActividadId == IdAct).FirstOrDefault(a => a.UserId == session);
            
            dbContext.Remove(RelationToDelete);
            dbContext.SaveChanges();

            return RedirectToAction("Home");
        } 
     // HANDLING DELETION OF ACTIVITY ~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("delete/{IdAct}")]
        public IActionResult deletewedding(int IdAct)
        {
            Actividad actividadToDelete = dbContext.Actividad.FirstOrDefault(X => X.ActividadId ==IdAct);
            dbContext.Remove(actividadToDelete);
            dbContext.SaveChanges();

            return RedirectToAction("Home");
        }
    }
}
