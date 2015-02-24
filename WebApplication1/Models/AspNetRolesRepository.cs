using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AspNetRolesRepository
    {
        public int AddOneRole(AspNetRole role)
        {
            EmployeesEntities db = new EmployeesEntities();
            db.AspNetRoles.Add(role);
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
        
        public IEnumerable<AspNetRole> GetAllAspNetRoles()
        {
            EmployeesEntities db = new EmployeesEntities();
            return db.AspNetRoles.ToList();
        }
    }
}