using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            //return View(careerProfileRepository.GetAllProfiles());
            return View();
        }
        public ActionResult JobSeekers()
        {
            return View();
        }
        public ActionResult Employers()
        {
            return View();
        }


        public ActionResult Search()
        {
            return View("Search");
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            IdentityUser identityUser = manager.Find(login.Email,
                                                             login.Password);

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ShowMsg", new { msg = "[ERR] invalid input" });
            }

            if (identityUser == null)
            {
                return RedirectToAction("ShowMsg", new { msg = "Sorry, try it again" });
            }

            IAuthenticationManager authenticationManager
                                    = HttpContext.GetOwinContext().Authentication;
            authenticationManager
            .SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var identity = new ClaimsIdentity(new[] {
                                    new Claim(ClaimTypes.Name, login.Email),
                                },
                                DefaultAuthenticationTypes.ApplicationCookie,
                                ClaimTypes.Name, ClaimTypes.Role);
            authenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, identity);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register register)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ShowMsg", new { msg = "[ERR] invalid input" });
            }

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            var identityUser = new IdentityUser()
            {
                UserName = register.Email,
                Email = register.Email
            };
            IdentityResult result = manager.Create(identityUser, register.Password);

            if (result.Succeeded)
            {
                var authenticationManager
                                  = HttpContext.Request.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(identityUser,
                                           DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { },
                                             userIdentity);
            }

            ProfileRepository profileRepository = new ProfileRepository();
            if (profileRepository.HandleRegistration(register) < 0)
            {
                return RedirectToAction("ShowMsg", new { msg = "[FAIL] HandleRegistration" });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ShowMsg(string msg)
        {
            ViewBag.msg = msg;
            return View();
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }




        //[HttpPost]
        //public ActionResult DisplaySearchResults()
        //{
        //    CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
        //    return View(careerProfileRepository.GetAllProfiles());
        //}

        public ActionResult IndividualProfile(int profileID)
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            return View(careerProfileRepository.FindProfile(profileID));
        }



        //public ActionResult ContactProfile()
        //{
        //    return View();
        //}

        //public ActionResult MessageSent()
        //{
        //    return View();
           
        //}
        //public ActionResult UserDelete() 
        //{
        //    return View();
        //}
	}
}