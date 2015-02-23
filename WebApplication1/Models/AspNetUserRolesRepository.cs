using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebApplication1.ViewModels;

namespace WebApplication1.Models
{
    public class AspNetUserRolesRepository
    {
        public int GetCountAspNetUserRole(string userID, string roleID)
        {
            EmployeesEntities db = new EmployeesEntities();
            
            int count = (from r in db.AspNetRoles
                        from u in r.AspNetUsers
                        where u.Id == userID && r.Id == roleID
                        select u.Id).Count();
            return count; 
        }

        public int AddAspNetUserRole(string userID, string roleID)
        {
            EmployeesEntities db = new EmployeesEntities();
            AspNetUser user = db.AspNetUsers
                             .Where(u => u.Id == userID).FirstOrDefault();
            AspNetRole role = db.AspNetRoles
                             .Where(r => r.Id == roleID).FirstOrDefault();
            user.AspNetRoles.Add(role);
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

        public int HandleAddUserToRole(string userID, string roleID)
        {
            if ( GetCountAspNetUserRole(userID, roleID) > 0 )
            {
                return 0; 
            }
            return AddAspNetUserRole(userID, roleID); 
        }

        public IEnumerable<UserRole> GetAllUserInRoles()
        {
            EmployeesEntities db = new EmployeesEntities();
            IEnumerable<UserRole> allUsersInRoles = (from r in db.AspNetRoles
                                                   from u in r.AspNetUsers
                                                   orderby r.Id, u.UserName
                                                  select new UserRole
                                                   {
                                                       UserName = u.UserName,
                                                       RoleName = r.Name
                                                   }).ToList<UserRole>();
            return allUsersInRoles;
        }
    }
}