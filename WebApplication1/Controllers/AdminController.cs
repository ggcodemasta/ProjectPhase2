using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult AdminHome()
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            return View(careerProfileRepository.GetAllProfiles());

        }
        public ActionResult Delete()
        {
            return View();
        }
    }
}