using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.ViewModels;
namespace WebApplication1.Models
{
    //TO DO LIST
    //convert year option to year max/min 
    //education counter
    //add platforms to db.skills

//CREATE TABLE Platform
//(
//    platformID INTEGER IDENTITY(1,1) PRIMARY KEY,
//    platform VARCHAR(250) NOT NULL
//);
    public class SearchRepo
    {
        public List<CareerProfile> SearchResults(string jobtitle, string industry, string country,
            string province, string city, string relocate, string education, string experience, List<string> platforms, List<string> languages)
        {
            List<Profile> asd = new List<Profile>();
            List<int> removeNonMatches = new List<int>();
            List<int> removeNonMatchesLanguges = new List<int>();

            EmployeesEntities context = new EmployeesEntities();

            int yearmin = 0;
            int yearmax = 0;
            switch (experience)
            {
                case "0":
                    yearmin = 0;
                    yearmax = 200;
                    break;
                case "1":
                    yearmin = 1;
                    yearmax = 2;
                    break;
                case "2":
                    yearmin = 3;
                    yearmax = 4;
                    break;
                case "3":
                    yearmin = 5;
                    yearmax = 10;
                    break;
                case "4":
                    yearmin = 10;
                    yearmax = 200;
                    break;
            }


            var filter1 = (from c in context.Careers
                          let p = c.Profile
                          from s in p.Skills
                          //where p.highestEducation == education
                          where c.years >= yearmin
                          where c.years <= yearmax
                          select p);
            if (jobtitle != "all") { 
                     
                    filter1 = (from p in filter1
                              from c in p.Careers
                              where c.jobTitle == jobtitle
                              select p);
            }

            if (industry != "all")
            {
                    filter1 = (from p in filter1
                              from c in p.Careers
                              where c.industry == industry
                              select p);
            }

            if (relocate != "on")
            {
                //try
                //{
                    var locationfilter = (from p in filter1
                                          where p.country == country
                                          where p.province == province
                                          where p.city == city
                                          select p).ToList();

                    asd.AddRange(locationfilter);
                //}
                //catch (ArgumentNullException ane)
                //{
                //    //to return?
                //    // string errormsg = "Sorry, no one matched your results in the given area, please try again.";
                //}
            }
            if (platforms[0] != "any")
            {

                List<int> ptfilterlist = new List<int>();

                
                for (int i = 0; i < platforms.Count; i++)
                {
                    var platformfilter = (from p in context.Profiles
                                          from pl in p.Platforms
                                          where pl.platform1 == platforms[i]
                                          select p.profileID).ToList();

                    ptfilterlist.AddRange(platformfilter);
                }
                removeNonMatches = ptfilterlist
                  .GroupBy(x => x)
                  .Where(g => g.Count() == platforms.Count)
                  .Select(g => g.Key).ToList();
            }

            if (languages[0] != "any")
            {

                List<int> languagesfilterlist = new List<int>();

                for (int i = 0; i < languages.Count; i++)
                {
                    var languagesfilter = (from p in asd
                                           from s in p.Skills
                                           where s.skillName == languages[i]
                                           select p.profileID).ToList();

                    languagesfilterlist.AddRange(languagesfilter);
                }
                removeNonMatchesLanguges = languagesfilterlist
                  .GroupBy(x => x)
                  .Where(g => g.Count() == languages.Count)
                  .Select(g => g.Key).ToList();
            }

            List<CareerProfile> candidates = new List<CareerProfile>();
            foreach (var item in removeNonMatchesLanguges)
            {
                var query = (from p in context.Profiles
                             where p.profileID == item
                             select p).FirstOrDefault();
                candidates.Add(new CareerProfile(query.profileID, query.firstName, query.lastName, query.linkedinURL,
                    query.portfolioURL, query.pictureURL, query.city, query.province, query.country,
                    query.highestEducation, query.relocationYN));
            }


            return candidates;

        }
    }
}