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
        // Auth2
        const string EMAIL_CONFIRMATION = "EmailConfirmation";
        const string PASSWORD_RESET = "ResetPassword";
        void CreateTokenProvider(UserManager<IdentityUser> manager, string tokenType)
        {
            manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();
        }
       
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
            DropdownPopulateRepo ddrepo = new DropdownPopulateRepo();
            ViewBag.jobtitles =   ddrepo.GetJobtitles();
          
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

        // auth2
        int ValidLogin(Login login)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };
            var user = userManager.FindByName(login.Email);

            if (user == null)
                return -1;

            // User is locked out.
            if (userManager.SupportsUserLockout && userManager.IsLockedOut(user.Id))
                return -2;

            // Validated user was locked out but now can be reset.
            // if (userManager.CheckPassword(user, login.Password))
            if (userManager.CheckPassword(user, login.Password)
                            && userManager.IsEmailConfirmed(user.Id))
            {
                if (userManager.SupportsUserLockout
                 && userManager.GetAccessFailedCount(user.Id) > 0)
                {
                    userManager.ResetAccessFailedCount(user.Id);
                }
            }
            // Login is invalid so increment failed attempts.
            else
            {
                bool lockoutEnabled = userManager.GetLockoutEnabled(user.Id);
                if (userManager.SupportsUserLockout && userManager.GetLockoutEnabled(user.Id))
                {
                    userManager.AccessFailed(user.Id);
                    return -3;
                }
                return -4; 
            }
            return 0;
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

            // if (identityUser == null)
            // auth2
            int res = ValidLogin(login);
            string message = ""; 
            if ( res == -1 )
            {
                message = "Sorry, but we don't have your data. Please, register first."; 
            }
            else if (res == -2)
            {
                message = "Sorry, but you are locked around 10 minutes due to previous 3 failed logins";
            }
            else if (res == -3)
            {
                message = "Sorry, but you didn't confirm your email address, or it was an invlid login.";
            }
            else if (res == -4)
            {
                message = "Sorry, but it was an invalid login. ";
            }

            if ( res < 0 ) 
            {
                return RedirectToAction("ShowMsg", new { msg = message });
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

            // var manager = new UserManager<IdentityUser>(userStore);
            // auth2
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };

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
        public ActionResult ShowMsg(string msg, string url="")
        {
            ViewBag.msg = msg;
            ViewBag.url = url; 
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

            // var manager = new UserManager<IdentityUser>(userStore);
            // auth2
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore)
            {
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = new TimeSpan(0, 10, 0),
                MaxFailedAccessAttemptsBeforeLockout = 3
            };

            var identityUser = new IdentityUser()
            {
                UserName = jobSeekers.Email,
                Email = jobSeekers.Email
            };
            IdentityResult result = manager.Create(identityUser, jobSeekers.Password);

            if (result.Succeeded)
            {
                /*
                var authenticationManager
                                  = HttpContext.Request.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(identityUser,
                                           DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { },
                                             userIdentity);
                 */
                // auth2
                CreateTokenProvider(manager, EMAIL_CONFIRMATION);

                var code = manager.GenerateEmailConfirmationToken(identityUser.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Home",
                                                new { userId = identityUser.Id, code = code },
                                                    protocol: Request.Url.Scheme);

                string email = "Please confirm your account by clicking this link: <a href=\""
                                + callbackUrl + "\">Confirm Registration</a>";

                MailHelper mailer = new MailHelper();
                string response = mailer.EmailFromArvixe(
                                           new Message("EmployeeArray", "EmployeeArray", "noreply@ea.com",
                                               "You need to comfirm this email", email, jobSeekers.Email));
                string message = "";
                if (response.IndexOf("Success", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    message = "We've emailed you. Please check it"; 
                }
                else
                {
                    message = "Sorry, we couldn't email you. Please retry it.";
                }

                // more...??
                // return RedirectToAction("ShowMsg", new { msg = message});
                return RedirectToAction("ShowMsg", new { msg = message, url = callbackUrl });
            }

            ProfileRepository profileRepository = new ProfileRepository();

            if (profileRepository.HandleJobSeekers(jobSeekers) < 0)
            {
                return RedirectToAction("ShowMsg", new { msg = "[FAIL] HandleRegistration" });
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmEmail(string userID, string code)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindById(userID);
            CreateTokenProvider(manager, EMAIL_CONFIRMATION);
            string message = ""; 
            try
            {
                IdentityResult result = manager.ConfirmEmail(userID, code);
                if (result.Succeeded)
                    message = "You are now registered!";
            }
            catch
            {
                message = "Validation attempt failed!";
            }

            return RedirectToAction("ShowMsg", new { msg = message });
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
            ViewBag.defaultvalue = currentProfile.educationID;

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
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("ShowMsg", new { msg = "[ERR] invalid input" });
            }

            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindByEmail(email);
            if (user == null)
            {
                return RedirectToAction("ShowMsg", new { msg = "Sorry, but we don't have your email." });
            }
            CreateTokenProvider(manager, PASSWORD_RESET);

            var code = manager.GeneratePasswordResetToken(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Home",
                                         new { userId = user.Id, code = code },
                                         protocol: Request.Url.Scheme);
            string emailContent = "Please reset your password by clicking <a href=\""
                                     + callbackUrl + "\"> Reset Password </a>";

            MailHelper mailer = new MailHelper();
            string response = mailer.EmailFromArvixe(
                                        new Message("EmployeeArray", "EmployeeArray", "noreply@ea.com",
                                            "You can reset your password.", emailContent, email));
            string message = "";
            if (response.IndexOf("Success", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                message = "We've emailed you. Please check it"; 
            }
            else
            {
                message = "Sorry, we couldn't email you. Please retry it.";
            }

            // more... ??
            // return RedirectToAction("ShowMsg");
            return RedirectToAction("ShowMsg", new { msg = message, url = callbackUrl });
        }

        [HttpGet]
        public ActionResult ResetPassword(string userID="", string code="")
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
                var user = manager.FindByEmail(HttpContext.User.Identity.Name);
                CreateTokenProvider(manager, PASSWORD_RESET);
                userID = user.Id; 
                code = manager.GeneratePasswordResetToken(user.Id);
            }

            AspNetUsersRepository aspNetUsersRepository = new AspNetUsersRepository();
            AspNetUser aspNetUser = aspNetUsersRepository.GetAspNetUserById(userID);
            ViewBag.Email = aspNetUser.Email; 
            ViewBag.PasswordToken = code;
            ViewBag.UserID = userID;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string password, string passwordConfirm,
                                          string passwordToken, string userID)
        {
            if (String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(passwordConfirm) ||
                String.IsNullOrWhiteSpace(passwordToken) || String.IsNullOrWhiteSpace(userID) )
            {
                return RedirectToAction("ShowMsg", new { msg = "[ERR] invalid input" });
            }

            CaptchaHelper captchaHelper = new CaptchaHelper();
            // string captchaResponse = captchaHelper.CheckRecaptcha();
            if ( captchaHelper.CheckRecaptcha() < 0 )
            {
                return RedirectToAction("ShowMsg", new { msg = "Sorry, but please click reCAPTCHA." });
            }
            
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindById(userID);
            CreateTokenProvider(manager, PASSWORD_RESET);

            IdentityResult result = manager.ResetPassword(userID, passwordToken, password);

            string message = "";
            if (result.Succeeded)
                message = "The password has been reset.";
            else
                message = "The password has not been reset.";

            return RedirectToAction("ShowMsg", new { msg = message });
        }
	}
}