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
    }
}