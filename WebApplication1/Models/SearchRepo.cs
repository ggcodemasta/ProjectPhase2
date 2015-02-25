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
            string province, string city, string relocate, string education, int yearmin, int yearmax, List<string> platforms, List<string> languages)
        {
            List<Profile> asd = new List<Profile>();
            List<int> removeNonMatches = new List<int>();
            List<int> removeNonMatchesLanguges = new List<int>();

            EmployeesEntities context = new EmployeesEntities();
            var withoutindustry = from c in context.Careers
                                  where c.jobTitle == jobtitle
                                  where c.years >= yearmin
                                  where c.years <= yearmax
                                  select c;
            if (industry != "all")
            {
                var withindustry = withoutindustry.Where(w => w.industry == industry);
            }

            var filter1 = from c in context.Careers
                          let p = c.Profile
                          from s in p.Skills
                          where c.jobTitle == jobtitle
                          where c.industry == industry
                          where p.highestEducation == education
                          where c.years >= yearmin
                          where c.years <= yearmax
                          select p;
            if (relocate == "no")
            {
                try
                {
                    var locationfilter = (from l in filter1
                                          where l.country == country
                                          where l.province == province
                                          where l.city == city
                                          select l).ToList();

                    asd.AddRange(locationfilter);
                }
                catch (ArgumentNullException ane)
                {
                    //to return?
                    // string errormsg = "Sorry, no one matched your results in the given area, please try again.";
                }
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