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
using WebApplication1.BusinessLogic;
using System.Data;
using System.Data.SqlClient;

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
        //[HttpPost]
        //public ActionResult Employers(string jobTitle, string city)
        //{
        //    CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
        //    IEnumerable<int> quickSearchResults = careerProfileRepository.QuickSearchProfiles(jobTitle, city);

        //    return RedirectToAction("DisplaySearchResults", "Home", new { displayList = quickSearchResults });
        //}

        [HttpGet]
        public ActionResult SearchError()
        {
            return View("SearchError");
        }

        public ActionResult Search()
        {
            DropdownPopulateRepo repo = new DropdownPopulateRepo();
            ViewBag.industries =  repo.GetIndustries();
            ViewBag.platforms = repo.GetPlatforms();
            ViewBag.jobtitles = repo.GetJobtitles();
            ViewBag.educations = repo.GetEducationList();
            ViewBag.skills = repo.GetSkills();
            return View("Search");
        }
        public ActionResult AdvancedSearchResults(string jobTitle, string industry,
                string country, string city, string province, string relocate, string education,
               string experience, List<string> platform, List<string> language)
        {
            SearchRepo srepo = new SearchRepo();
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            ViewBag.PremiumUsers = careerProfileRepository.GetAllPremiumProfiles();
            
            try
            {
                List<CareerProfile> results = srepo.
                    AdvancedSearchQuery(jobTitle, industry, country, province, city, relocate, education, experience, platform, language);
                if (results.Count < 1) 
                {
                    return View("SearchError"); 
                }
                return View(results);
            }
            catch (NullReferenceException nre)
            {
                return View("SearchError");
            }
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

        [Authorize]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult DisplaySearchResults(string jobTitle, string city)
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            ViewBag.PremiumUsers = careerProfileRepository.GetAllPremiumProfiles();

            IEnumerable<CareerProfile> returnList = careerProfileRepository.QuickSearchProfiles(jobTitle, city);

            return View(returnList);
        }

        public ActionResult DisplaySearchResults()
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            ViewBag.PremiumUsers = careerProfileRepository.GetAllPremiumProfiles();

            return View(careerProfileRepository.GetAllProfiles());
        }


        public ActionResult IndividualProfile(int profileID)
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            return View(careerProfileRepository.FindProfile(profileID));
        }


        //public ActionResult UserDelete() 
        //{
        //    return View();
        //}


        [HttpPost]
        public ActionResult JobSeekers(JobSeekers jobSeekers)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ShowMsg", new { msg = "[ERR] invalid input" });
            }

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            var identityUser = new IdentityUser()
            {
                UserName = jobSeekers.Email,
                Email = jobSeekers.Email
            };
            IdentityResult result = manager.Create(identityUser, jobSeekers.Password);

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

            if (profileRepository.HandleJobSeekers(jobSeekers) < 0)
            {
                return RedirectToAction("ShowMsg", new { msg = "[FAIL] HandleRegistration" });
            }

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Career()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Career(Careers careers)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ShowMsg", new { msg = "[ERR] invalid input" });
            }


            CareerRepository careerRepository = new CareerRepository();

            careers.CareerID = 0;
            string email = HttpContext.User.Identity.Name; // userid

            EmployeesEntities db = new EmployeesEntities();
            int profileID = (from p in db.Profiles
                             where p.email == email
                             select p.profileID).SingleOrDefault();


            careers.ProfileID = profileID;


            if (careerRepository.HandleCareer(careers) < 0)
            {
                return RedirectToAction("ShowMsg", new { msg = "[FAIL] HandleCareer" });
            }

            return RedirectToAction("ViewCareers", "Home");
        }

        public ActionResult ViewCareers()
        {
            CareerRepository careerRepository = new CareerRepository();

            string email = HttpContext.User.Identity.Name; // userid

            EmployeesEntities db = new EmployeesEntities();
            int profileID = (from p in db.Profiles
                             where p.email == email
                             select p.profileID).SingleOrDefault();

            // int profileID = 1;

            IEnumerable<Career> allCareers = careerRepository.GetManyCareers(profileID);


            return View(allCareers);
        }


        [HttpGet]
        public ActionResult ContactProfile(string profileID, string firstName, string lastName)
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            ViewBag.PremiumUsers = careerProfileRepository.GetAllPremiumProfiles();

            ViewBag.profileID = profileID;
            ViewBag.firstName = firstName;
            ViewBag.lastName = lastName;

            return View();
        }

        [HttpPost]
        public ActionResult ContactProfile(string profileID, string senderName, string companyName, string senderEmail, string subject, string message, string firstName, string lastName)
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            ViewBag.PremiumUsers = careerProfileRepository.GetAllPremiumProfiles();

            ProfileRepository pr = new ProfileRepository();
            string sendToEmail = pr.GetProfileEmail(Int32.Parse(profileID));

            MailHelper mailer = new MailHelper();
            string response = mailer.EmailFromArvixe(
                                       new Message(senderName, companyName, senderEmail, subject, message, sendToEmail));

            if (response.IndexOf("Success", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                return RedirectToAction("MessageSent", "Home");
            }
            else
            {
                ViewBag.Response = "Error - Message Not Sent: " + response;

                ViewBag.profileID = profileID;
                ViewBag.firstName = firstName;
                ViewBag.lastName = lastName;
                return View();
            }
            
        }

        public ActionResult MessageSent()
        {
            CareerProfileRepository careerProfileRepository = new CareerProfileRepository();
            ViewBag.PremiumUsers = careerProfileRepository.GetAllPremiumProfiles();

            return View();

        }


        public ActionResult WebAPI()
        {

            return View();
        }

        [Authorize]
        public ActionResult BasicInfo()
        {
            ProfileRepository profileRepository = new ProfileRepository();
            DropdownPopulateRepo repo = new DropdownPopulateRepo();
            SearchRepo srepo = new SearchRepo();
            string email = HttpContext.User.Identity.Name; // userid
            Profile currentProfile = profileRepository.GetProfile(email);
            ViewBag.educations = repo.GetEducationList();
            ViewBag.defaultvalue = srepo.GetEducation(currentProfile.educationID);

            return View(currentProfile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult BasicInfo(Profile profile)
        {


            ProfileRepository profileRepository = new ProfileRepository();
            SearchRepo srepo = new SearchRepo();
            string email = HttpContext.User.Identity.Name; // userid
            int education = srepo.SaveEducation(Request.Form["education"]);


            if (profileRepository.SaveProfile(profile, email, education) < 0)
            {
                return RedirectToAction("ShowMsg", new { msg = "[FAIL] HandleCareer" });
            }

            return RedirectToAction("Career", "Home");
        }

        [HttpGet]
        public ActionResult Skills()
        {
            EmployeesEntities db = new EmployeesEntities();

            string email = HttpContext.User.Identity.Name; // userid

            var query = from p in db.Profiles
                        from s in p.Skills
                        where p.email == email
                        select new
                        {
                            SkillID = s.skillID,
                            SkillName = s.skillName
                        };

            //   if(profile.Skills.Contains(skill))
            Skills skills = new Skills();
            foreach (var profileSkill in query)
            {
                switch (profileSkill.SkillName)
                {
                    case "Ajax":
                        skills.Ajax = true;
                        break;
                    case "AngularJS":
                        skills.AngularJS = true;
                        break;
                    case "AspectC++":
                        skills.AspectCPlusPlus = true;
                        break;
                    case "Assembly":
                        skills.Assembly = true;
                        break;
                    case "ASP.NET":
                        skills.AspdotNet = true;
                        break;
                    case "BASIC":
                        skills.BASIC = true;
                        break;
                    case "C":
                        skills.C = true;
                        break;
                    case "C++":
                        skills.CPlusPlus = true;
                        break;
                    case "C#":
                        skills.CSharp = true;
                        break;
                    case "C-RIMM":
                        skills.C_RIMM = true;
                        break;
                    case "CSS":
                        skills.CSS = true;
                        break;
                    case "Fortran":
                        skills.Fortran = true;
                        break;
                    case "Java":
                        skills.Java = true;
                        break;
                    case "Javascript":
                        skills.Javascript = true;
                        break;
                    case "MySQL":
                        skills.MySQL = true;
                        break;
                    case "NodeJS":
                        skills.NodeJS = true;
                        break;
                    case "Objective C":
                        skills.ObjectiveC = true;
                        break;
                    case "Perl":
                        skills.Perl = true;
                        break;
                    case "PHP":
                        skills.PHP = true;
                        break;
                    case "Python":
                        skills.Python = true;
                        break;
                    case "Ruby":
                        skills.Ruby = true;
                        break;
                    case "SQL/Variations":
                        skills.SQLVariations = true;
                        break;
                    case "Visual Basic":
                        skills.VisualBasic = true;
                        break;
                    case "XML":
                        skills.XML = true;
                        break;
                    case "Laravel":
                        skills.Laravel = true;
                        break;
                    case "Adobe Suite":
                        skills.AdobeSuite = true;
                        break;
                    case "JQuery":
                        skills.JQuery = true;
                        break;
                    case "Json":
                        skills.Json = true;
                        break;
                    case "Twitter BootStrap":
                        skills.TwitterBootStrap = true;
                        break;

                }
            }

            return View(skills);
        }





        [HttpPost]
        public ActionResult Skills(Skills skills)
        {
            string email = HttpContext.User.Identity.Name; // userid

            EmployeesEntities db = new EmployeesEntities();
            int profileID = (from p in db.Profiles
                             where p.email == email
                             select p.profileID).SingleOrDefault();

            EmployeesEntities context = new EmployeesEntities();
            ProfileRepository profileRepository = new ProfileRepository();

            if (skills.Ajax)
            { profileRepository.AddOneSkill(profileID, "Ajax"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Ajax"); }
            if (skills.AngularJS)
            { profileRepository.AddOneSkill(profileID, "AngularJS"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "AngularJS"); }
            if (skills.AspectCPlusPlus)
            { profileRepository.AddOneSkill(profileID, "AspectC++"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "AspectC++"); }
            if (skills.Assembly)
            { profileRepository.AddOneSkill(profileID, "Assembly"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Assembly"); }
            if (skills.AspdotNet)
            { profileRepository.AddOneSkill(profileID, "ASP.NET"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "ASP.NET"); }
            if (skills.BASIC)
            { profileRepository.AddOneSkill(profileID, "BASIC"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "BASIC"); }
            if (skills.C)
            { profileRepository.AddOneSkill(profileID, "C"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "C"); }
            if (skills.CPlusPlus)
            { profileRepository.AddOneSkill(profileID, "C++"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "C++"); }
            if (skills.CSharp)
            { profileRepository.AddOneSkill(profileID, "C#"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "C#"); }
            if (skills.C_RIMM)
            { profileRepository.AddOneSkill(profileID, "C-RIMM"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "C-RIMM"); }
            if (skills.CSS)
            { profileRepository.AddOneSkill(profileID, "CSS"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "CSS"); }
            if (skills.Fortran)
            { profileRepository.AddOneSkill(profileID, "Fortran"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Fortran"); }

            if (skills.Java)
            { profileRepository.AddOneSkill(profileID, "Java"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Java"); }
            if (skills.Javascript)
            { profileRepository.AddOneSkill(profileID, "Javascript"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Javascript"); }
            if (skills.MySQL)
            { profileRepository.AddOneSkill(profileID, "MySQL"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "MySQL"); }
            if (skills.NodeJS)
            { profileRepository.AddOneSkill(profileID, "NodeJS"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "NodeJS"); }
            if (skills.ObjectiveC)
            { profileRepository.AddOneSkill(profileID, "Objective C"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Objective C"); }
            if (skills.Perl)
            { profileRepository.AddOneSkill(profileID, "Perl"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Perl"); }
            if (skills.PHP)
            { profileRepository.AddOneSkill(profileID, "PHP"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "PHP"); }
            if (skills.Python)
            { profileRepository.AddOneSkill(profileID, "Python"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Python"); }
            if (skills.Ruby)
            { profileRepository.AddOneSkill(profileID, "Ruby"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Ruby"); }
            if (skills.SQLVariations)
            { profileRepository.AddOneSkill(profileID, "SQL/Variations"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "SQL/Variations"); }

            if (skills.VisualBasic)
            { profileRepository.AddOneSkill(profileID, "Visual Basic"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Visual Basic"); }
            if (skills.XML)
            { profileRepository.AddOneSkill(profileID, "XML"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "XML"); }
            if (skills.Laravel)
            { profileRepository.AddOneSkill(profileID, "Laravel"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Laravel"); }
            if (skills.AdobeSuite)
            { profileRepository.AddOneSkill(profileID, "Adobe Suite"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Adobe Suite"); }
            if (skills.JQuery)
            { profileRepository.AddOneSkill(profileID, "JQuery"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "JQuery"); }
            if (skills.Json)
            { profileRepository.AddOneSkill(profileID, "Json"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Json"); }
            if (skills.TwitterBootStrap)
            { profileRepository.AddOneSkill(profileID, "Twitter BootStrap"); }
            else
            { profileRepository.DeleteOneSkill(profileID, "Twitter BootStrap"); }
            return View();
        }
      
      

	}
}