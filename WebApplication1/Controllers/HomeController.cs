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

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }






        //[HttpPost]
        //public ActionResult DisplaySearchResults()
        //{
        //    CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
        //    return View(careerProfileRepository.GetAllProfiles());
        //}

        //public ActionResult IndividualProfile(int profileID)
        //{
        //    CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
        //    return View(careerProfileRepository.FindProfile(profileID));
        //}



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