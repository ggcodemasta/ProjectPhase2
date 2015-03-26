using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.ViewModels;

namespace WebApplication1.Models
{
    public class CareerRepository
    {
        public int GetProfileID(Login login)
        {

            EmployeesEntities db = new EmployeesEntities();
            int profileID = (from p in db.Profiles
                             where p.email == login.Email && p.password == login.Password
                             select p.profileID).SingleOrDefault();
            return profileID;
        }


        public int AddCareer(Career career)
        {

            EmployeesEntities db = new EmployeesEntities();

            db.Careers.Add(career);
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

        public int HandleCareer(Careers careers)
        {
            Career career = new Career();

          //  career.careerID = careers.CareerID;

            career.profileID = careers.ProfileID;

            career.industry = careers.Industry;
            career.company = careers.Company;
            career.jobTitle = careers.JobTitle;
            career.years = careers.Years;
            career.description = careers.Description;
            return AddCareer(career);

        }



        public IEnumerable<Career> GetAllCareers()
        {
            EmployeesEntities db = new EmployeesEntities();
            IEnumerable<Career> careers = db.Careers;
            return careers;

        }

        public IEnumerable<Career> GetManyCareers(int profileID)
        {
            EmployeesEntities db = new EmployeesEntities();
            IEnumerable<Career> careers = db.Careers.Where(r => r.profileID == profileID).ToList();
            return careers;
        }


    }
}