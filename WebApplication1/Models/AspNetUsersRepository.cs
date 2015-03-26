using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AspNetUsersRepository
    {
        public IEnumerable<AspNetUser> GetAllAspNetUsers()
        {
            EmployeesEntities db = new EmployeesEntities();
            return db.AspNetUsers.ToList();
        }

        public AspNetUser GetAspNetUserById(string Id)
        {
            EmployeesEntities db = new EmployeesEntities();
            return db.AspNetUsers.Where(r => r.Id == Id).SingleOrDefault();
        }
    }
}