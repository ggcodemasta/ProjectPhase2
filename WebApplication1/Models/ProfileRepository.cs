using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.ViewModels;

namespace WebApplication1.Models
{
    public class ProfileRepository
    {
        public int GetProfileID(Login login)
        {

            EmployeesEntities db = new EmployeesEntities();
            int profileID = (from p in db.Profiles
                             where p.email == login.Email && p.password == login.Password
                             select p.profileID).SingleOrDefault();
            return profileID;
        }


        public int AddProfile(Profile profile)
        {
            EmployeesEntities db = new EmployeesEntities();

            db.Profiles.Add(profile);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return -1;
            }
            return 1;
        }

        public int HandleRegistration(Register register)
        {
            Profile profile = new Profile();

            profile.email = register.Email;
            // more... 
            profile.password = "none";
            profile.firstName = register.FirstName;
            profile.lastName = register.LastName;
            profile.linkedinURL = register.LinkedinURL;
            profile.pictureURL = register.PictureURL;
            profile.portfolioURL = register.PortfolioURL;
            profile.highestEducation = register.HighestEducation;
            profile.relocationYN = register.RelocationYN;
            profile.country = register.Country;
            profile.province = register.Province;
            profile.city = register.City;

            return AddProfile(profile);

        }
    }
}