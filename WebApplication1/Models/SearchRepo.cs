﻿using System;
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
   
        public String GetEducation(int? profile) 
        {
            var query = (from p in context.Profiles
                         from e in context.Educations
                         where p.educationID == e.educationID
                         select e.educationName).FirstOrDefault();
            return query;
        }
        public String GetEducation(int profile)
        {
            var query = (from p in context.Profiles
                         from e in context.Educations
                         where p.educationID == e.educationID
                         select e.educationName).FirstOrDefault();
            return query;
        }
        public int SaveEducation(String education)
        {
            int id = 0;
                         List<Education> allRows = context.Educations.ToList();
                         foreach (var row in allRows) {
                             if (education == row.educationName) {
                                 id = row.educationID;
                             }
                         }

            return id;
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


                int edu = SaveEducation(education);
            //education
                IQueryable<Profile> filtereducation = (from p in context.Profiles
                                                       where p.educationID >= edu
                                       select p);
            //years experience
                IQueryable<Profile> filteryears = (from p in filtereducation
                                                   from c in p.Careers
                                                   where c.years >= yearmin
                                                   where c.years <= yearmax
                                                   select p).Distinct();
            //jobtitle
                IQueryable<Profile> filterjobtitles;
                if (jobtitle != "ALL")
                {
                    filterjobtitles = (from p in filteryears
                                       from c in p.Careers
                                       where c.jobTitle == jobtitle
                                       select p);
                }
                else
                {
                    filterjobtitles = filteryears;
                }
            
            
            //industry
                IQueryable<Profile> filterindustry;
                if (industry != "ALL")
                {
                    filterindustry = (from p in filterjobtitles
                                      from c in p.Careers
                                      where c.industry == industry
                                      select p);
                }
                {
                    filterindustry = filterjobtitles;
                }


                List<Profile> filtercountry = new List<Profile>();
                List<Profile> filterprovince = new List<Profile>();
                List<Profile> filtercity = new List<Profile>();
            IQueryable<Profile> filterrelocation;
            //relocate

            if (country != "")
            {

                filtercountry = (from p in filterindustry
                                 where p.country.ToLower() == country.ToLower()
                                 select p).ToList();
            }
            else 
            {
                filtercountry = filterindustry.ToList();
            }


            if (province != "")
            {
                filterprovince = (from p in filtercountry
                                  where p.province.ToLower() == province.ToLower()
                                  select p).ToList();
            }
            else { filterprovince = filtercountry; }

            if (city != "")
            {

                filtercity = (from p in filterprovince
                              where p.city.ToLower() == city.ToLower()
                              select p).ToList();
            }
            else { filtercity = filterprovince; }
                 
                    if (relocate == "on")
                    {
                        //query for those not in area but willing to relocate
                        filterrelocation = (from p in filterindustry
                                            where p.city.ToLower() != city.ToLower()
                                            where p.province.ToLower() != province.ToLower()
                                            where p.relocationYN.ToLower() == "yes"
                                            select p);
                        profilelist.AddRange(filterrelocation);
                        profilelist.AddRange(filtercity);
                    }
                    else {
                        profilelist.AddRange(filtercity);
                    }
                    
                    
        
                if (platforms[0] != "ALL")
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
            
                if (languages[0] != "ALL")
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
                CareerProfileRepository cprepo = new CareerProfileRepository();
            candidates = cprepo.GetSkills(candidates);
                candidates = cprepo.GetCareers(candidates);
                return candidates;

        }


    }
}