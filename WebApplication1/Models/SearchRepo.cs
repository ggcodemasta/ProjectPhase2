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
        EmployeesEntities context = new EmployeesEntities();
        public String GetEducation(int? profileID) 
        {
            var query = (from p in context.Profiles
                         from e in context.Educations
                         where p.educationID == e.educationID
                         select e.educationName).FirstOrDefault();
            return query;
        }
        public int SaveEducation(String education)
        {
            var query = (from p in context.Profiles
                         let e = p.Education
                         where e.educationName == education
                         select e.educationID).FirstOrDefault();
            return query;
        }
        public List<CareerProfile> AdvancedSearchQuery(string jobtitle, string industry, string country,
            string province, string city, string relocate, string education, string experience, List<string> platforms, List<string> languages)
        {
            List<Profile> profilelist = new List<Profile>();
            List<int> removenonmatchesindustry = new List<int>();
            List<int> removeNonMatchesLanguges = new List<int>();
            //string relocation ="";
            //if (relocate != null)
            //{
            //    relocation = "yes";
            //}else {
            //    relocation = "no";
            //}
          

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

                var filterbasic = (from c in context.Careers
                                   let p = c.Profile
                                   where c.years >= yearmin
                                   where c.years <= yearmax
                                   select p).Distinct();

                IQueryable<Profile> filterjobtitles;
                if (jobtitle != "all")
                {
                    filterjobtitles = (from p in filterbasic
                                       from c in p.Careers
                                       where c.jobTitle == jobtitle
                                       select p);
                }
                else
                {
                    filterjobtitles = filterbasic;
                }

                IQueryable<Profile> filterindustry;
                if (industry != "all")
                {
                    filterindustry = (from p in filterjobtitles
                                      from c in p.Careers
                                      where c.industry == industry
                                      select p);
                }
                {
                    filterindustry = filterjobtitles;
                }

                IQueryable<Profile> filterlocation1;

                if (relocate == "on")
                {
                    //query for those in the area
                    filterlocation1 = (from p in filterindustry
                                      where p.country == country
                                      where p.province == province
                                      where p.city == city 
                                      select p);
                    //query for those not in area but willing to relocate
                    var filterlocation2 = (from p in filterindustry
                                       where p.relocationYN == "yes"
                                       select p).ToList();
                    filterlocation2.AddRange(filterlocation1);
                        profilelist.AddRange(filterlocation2);
                  
                }
                else
                {//only get those in the area
                    filterlocation1 = (from p in filterindustry
                                      where p.country == country
                                      where p.province == province
                                      where p.city == city
                                      select p);
                    profilelist.AddRange(filterlocation1);
                }

                if (platforms[0] != "all")
                {

                    List<int> ptfilterlist = new List<int>();
                    for (int i = 0; i < platforms.Count; i++)
                    {
                        var platformfilter = (from p in profilelist
                                              from pl in p.Platforms
                                              where pl.platformName == platforms[i]
                                              select p.profileID).ToList();

                        ptfilterlist.AddRange(platformfilter);
                    }
                    removenonmatchesindustry = ptfilterlist
                     .GroupBy(x => x)
                      .Where(g => g.Count() == platforms.Count)
                      .Select(g => g.Key).ToList();
                }
                else
                {
                    var platformfilter = (from p in profilelist
                                          select p.profileID).ToList();
                    removenonmatchesindustry.AddRange(platformfilter);
                }
            
                if (languages[0] != "all")
                {
                    List<Profile> int2profile = new List<Profile>();
                    foreach (int profileid in removenonmatchesindustry)
                    {
                        var platformtolanguage = (from p in context.Profiles
                                                  where p.profileID == profileid
                                                  select p).FirstOrDefault();
                        int2profile.Add(platformtolanguage);

                    }
                    List<int> languagesfilterlist = new List<int>();

                    for (int i = 0; i < languages.Count; i++)
                    {
                        var languagesfilter = (from p in int2profile
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
                else 
                {
                    removeNonMatchesLanguges = removenonmatchesindustry;
                }

                List<CareerProfile> candidates = new List<CareerProfile>();
                foreach (var item in removeNonMatchesLanguges)
                {
                    var query = (from p in context.Profiles
                                 where p.profileID == item
                                 select p).FirstOrDefault();

                    if (query.pictureURL == null || query.pictureURL.IndexOf(".jpg", 0, StringComparison.CurrentCultureIgnoreCase) == -1 && query.pictureURL.IndexOf(".png", 0, StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        query.pictureURL = "/Content/images/person-placeholder2.jpg";
                    }

                    candidates.Add(new CareerProfile(query.profileID, query.firstName, query.lastName, query.linkedinURL,
                        query.portfolioURL, query.pictureURL, query.city, query.province, query.country,
                        GetEducation(query.profileID), query.relocationYN));
                }
                
                return candidates;

        }


    }
}