using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserLoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Career()
        {
            return View();
        }
        public ActionResult Subscribe()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Premium()
        {
            SvcTypeRepository svcTypeRepository = new SvcTypeRepository();
            return View(svcTypeRepository.GetAllSvcType());
        }
    }
}