using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebApplication1.Models;
using WebApplication1.ViewModels; 

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

        [HttpGet]
        public ActionResult AddRole()
        {
            AspNetRolesRepository aspNetRolesRepository = new AspNetRolesRepository();
            ViewBag.allRoles = aspNetRolesRepository.GetAllAspNetRoles(); 
            return View(); 
        }

        [HttpPost]
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

    }
}