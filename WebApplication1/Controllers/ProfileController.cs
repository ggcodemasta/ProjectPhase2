using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Career()
        {
            return View(); 
        }
        public ActionResult Suscribe() 
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}