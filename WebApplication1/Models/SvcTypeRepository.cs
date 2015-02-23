using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SvcTypeRepository
    {
        public IEnumerable<SvcType> GetAllSvcType()
        {
            EmployeesEntities db = new EmployeesEntities();
            return db.SvcTypes.OrderBy(r => r.svcTypeID).ToList();
        }

        public SvcType GetOneSvcType(int svcTypeID)
        {
            EmployeesEntities db = new EmployeesEntities();
            return db.SvcTypes.Where(r => r.svcTypeID == svcTypeID).SingleOrDefault();
        }
    }
}