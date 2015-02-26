using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList; 

using WebApplication1.Models;
using WebApplication1.ViewModels; 

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles="Admin")]
        public ActionResult AdminHome()
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            return View(careerProfileRepository.GetAllProfiles());

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRole()
        {
            AspNetRolesRepository aspNetRolesRepository = new AspNetRolesRepository();
            ViewBag.allRoles = aspNetRolesRepository.GetAllAspNetRoles(); 
            return View(); 
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRole(AspNetRole role)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ShowMsg", "Home",  new { msg = "[ERR] invalid input" });
            }

            AspNetRolesRepository aspNetRolesRepository = new AspNetRolesRepository();
            if (aspNetRolesRepository.AddOneRole(role) <= 0 )
            {
                return RedirectToAction("ShowMsg", "Home", new { msg = "[FAIL] AddOneRole" });
            }

            return RedirectToAction("AddRole");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddUserToRole()
        {
            AspNetUserRolesRepository aspNetUserRolesRepository = new AspNetUserRolesRepository();
            ViewBag.allUsersInRoles = aspNetUserRolesRepository.GetAllUserInRoles();

            AspNetUsersRepository aspNetUsersRepository = new AspNetUsersRepository();
            ViewBag.allUsers = aspNetUsersRepository.GetAllAspNetUsers();

            AspNetRolesRepository aspNetRolesRepository = new AspNetRolesRepository();
            ViewBag.allRoles = aspNetRolesRepository.GetAllAspNetRoles(); 

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddUserToRole(string userID, string roleID)
        {
            if ( String.IsNullOrWhiteSpace(userID) && String.IsNullOrWhiteSpace(roleID) )
            {
                return RedirectToAction("ShowMsg", "Home", new { msg = "[ERR] invalid input" });
            }

            AspNetUserRolesRepository aspNetUserRolesRepository = new AspNetUserRolesRepository();
            if (aspNetUserRolesRepository.HandleAddUserToRole(userID, roleID) < 0)
            {
                return RedirectToAction("ShowMsg", "Home", new { msg = "[FAIL] HandleAddUserToRole" });
            }

            return RedirectToAction("AddUserToRole");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ShowAllUser(int? page)
        {
            const int PAGE_SIZE = 10;
            int pageNumber = (page ?? 1);

            ProfileRepository profileRepository = new ProfileRepository();
            IEnumerable<Profile> allProfiles = profileRepository.GetAllProfiles();
            allProfiles = allProfiles.ToPagedList(pageNumber, PAGE_SIZE);

            return View(allProfiles); 
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ShowAllPremiumUser(int? page)
        {
            const int PAGE_SIZE = 10;
            int pageNumber = (page ?? 1);

            SvcProfileRepository svcProfileRepository = new SvcProfileRepository();
            IEnumerable<SvcProfileDetails> allSvcProfileDetails = svcProfileRepository.GetAllSvcProfileDetails();
            allSvcProfileDetails = allSvcProfileDetails.ToPagedList(pageNumber, PAGE_SIZE);

            return View(allSvcProfileDetails); 
        }

    }
}