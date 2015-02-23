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
    }
}