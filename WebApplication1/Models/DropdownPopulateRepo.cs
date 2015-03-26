using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DropdownPopulateRepo
    {
        EmployeesEntities context = new EmployeesEntities();

        public List<String> GetIndustries()
        {
            List<String> industryList = new List<string>();

            IEnumerable<String> industries = (from i in context.Industries
                                              select i.industryName).ToList();
            industryList.AddRange(industries);
            industryList.Sort();
            return industryList;
           
        }
        public List<String> GetSkills()
        {
            List<String> skillslist = new List<string>();

            IEnumerable<String> skills = (from s in context.Skills
                                          select s.skillName).ToList();
            skillslist.AddRange(skills);
            skillslist.Sort();
            return skillslist;
        }
        public List<String> GetJobtitles()
        {
            List<String> jobtitleList = new List<string>();

            IEnumerable<String> jobtitles = (from j in context.Jobtitles
                                          select j.jobTitle1).ToList();
            jobtitleList.AddRange(jobtitles);
            jobtitleList.Sort();
            return jobtitleList;
        }
        public List<String> GetPlatforms()
        {
            List<String> platformList = new List<string>();

            IEnumerable<String> ptlist = (from pt in context.Platforms
                                             select pt.platformName).ToList();
            platformList.AddRange(ptlist);
            platformList.Sort();
            return platformList;
        }

        public List<String> GetEducationList()
        {
            List<String> eduList = new List<string>();

            IEnumerable<String> query = (from e in context.Educations
                                              select e.educationName).ToList();
            eduList.AddRange(query);
            return eduList;

        }

    }
}