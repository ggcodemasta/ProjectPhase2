using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        //Grant test commit
        public ActionResult Index()
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            return View(careerProfileRepository.GetAllProfiles());
        }

        [HttpPost]
        public ActionResult DisplaySearchResults()
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            return View(careerProfileRepository.GetAllProfiles());
        }

        public ActionResult IndividualProfile(int profileID)
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            return View(careerProfileRepository.FindProfile(profileID));
        }

        public ActionResult Search()
        {
            return View("Search");
        }
        public ActionResult About() 
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult ContactProfile()
        {
            return View();
        }

        public ActionResult MessageSent()
        {
            return View();
           
        }
        public ActionResult UserDelete() 
        {
            return View();
        }
	}
}