using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    //TO DO LIST
    //convert year option to year max/min 
    //bool relocate
    //education counter


    public class SearchRepo
    {
        public void SearchResults(string jobtitle, string industry, string country,
            string province, string city, bool relocate, int educationcounter, int yearmin, int yearmax)
        {
            EmployeesEntities context = new EmployeesEntities();
            var withoutindustry = from c in context.Careers
                                where c.jobTitle == jobtitle
                                where c.years >= yearmin
                                where c.years <= yearmax
                                select c;
            if(industry != "all"){
                var withindustry = withoutindustry.Where(w => w.industry == industry);
            }

            var profiles = from p in context.Profiles
                           where p.country == country
                           where p.province == province
                           where p.city == city
                           //where p.relocationbool == relocate
                           //where p.educationCounter >= educationcounter
                           select p;
                          
            //filters by industries, jobtitle, years from both tables
            var  = withoutindustry.Where(w => w.industry == industry);
                          
        }
    }
}