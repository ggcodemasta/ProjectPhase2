using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class x_LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(z_Login login)
        {
            if (ModelState.IsValid)
            {
                ViewBag.msg = "ModelState.IsValid is TRUE";
                ProfileRepository PR = new ProfileRepository();
                int profileID = PR.GetProfileID(login);

                ViewBag.profileID = profileID;
                // ??? more ... redirect  if ID > 0 or ID == 0 
            }
            else
            {
                ViewBag.msg = "ModelState.IsValid is FALSE";
            }

            return View();
        }

        public ActionResult AdminHome()
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            return View(careerProfileRepository.GetAllProfiles());
        
        }

        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Delete()
        {
            return View();
        }
    }
}