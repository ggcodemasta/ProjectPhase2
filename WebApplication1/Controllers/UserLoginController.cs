using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApplication1.ViewModels;
using WebApplication1.Models;
using WebApplication1.BusinessLogic;

namespace WebApplication1.Controllers
{
    public class UserLoginController : Controller
    {
        public ActionResult Index()
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
        [Authorize]
        public ActionResult Premium()
        {
            SvcTypeRepository svcTypeRepository = new SvcTypeRepository();
            return View(svcTypeRepository.GetAllSvcType());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Premium(int svcTypeID = 0)
        {
            if (svcTypeID <= 0 )
            {
                 return RedirectToAction("ShowMsg", "Home",  new { msg = "[ERR] invalid input" });
            }

            SvcProfileRepository svcProfileRepository = new SvcProfileRepository();
            if ( svcProfileRepository.HandleAddingSvcProfile(svcTypeID, "Credit Card") < 0)
            {
                return RedirectToAction("ShowMsg", "Home", new { msg = "[FAIL] HandleAddingSvcProfile" });
            }

            return RedirectToAction("MyPremium"); 
        }

        [Authorize]
        public ActionResult MyPremium(int? page)
        {
            const int ROW_CNT_PER_PAGE = 10; 
            int pageNumber = (page ?? 1);

            SvcProfileRepository svcProfileRepository = new SvcProfileRepository();
            IEnumerable<SvcProfile> mySvcProfiles = svcProfileRepository.HandleGettingManySvcProfilesByProfileID();
            return View(mySvcProfiles.ToPagedList(pageNumber, ROW_CNT_PER_PAGE)); 
        }

        public ActionResult PaypalSuccess()
        {
            return RedirectToAction("MyPremium"); 
        }

        public ActionResult Paypal_IPN()
        {
            Paypal_IPN paypalIPN = new Paypal_IPN("test");
            if (paypalIPN.TXN_ID != null)
            {
                string[] buff = paypalIPN.OptionSelection1.Split('.');
                int svcTypeID = Int32.Parse(buff[0]);
                SvcProfileRepository svcProfileRepository = new SvcProfileRepository();
                svcProfileRepository.HandleAddingSvcProfile(svcTypeID, "PayPal", paypalIPN.Custom);
                ViewBag.msg = "IPN is Not Null";
            }
            else
            {
                ViewBag.msg = "IPN.TXN_ID is null";
            }

            /*
            // TEST
            string optionSelection1 = "1.Basic Service (4 weeks)";
            string[] buff = optionSelection1.Split('.');
            int svcTypeID = Int32.Parse(buff[0]);
            SvcProfileRepository svcProfileRepository = new SvcProfileRepository();
            svcProfileRepository.HandleAddingSvcProfile(svcTypeID, "PayPal", "user1@gmail.com");
            ViewBag.msg = "TEST"; */

            return View(); 
        }
    }
}