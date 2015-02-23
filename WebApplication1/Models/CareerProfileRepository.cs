using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.ViewModels;

namespace WebApplication1.Models
{
    public class CareerProfileRepository
    {

        public CareerProfile FindProfile(int profileID)
        {
            EmployeesEntities context = new EmployeesEntities();

            IEnumerable<Profile> query = (from p in context.Profiles
                         where p.profileID == profileID
                         select p);

            List<CareerProfile> bac = new List<CareerProfile>();
            foreach (var item in query)
            {
                bac.Add(new CareerProfile(item.profileID, item.firstName, item.lastName, item.linkedinURL,
                    item.portfolioURL, item.pictureURL, item.city, item.province, item.country,
                    item.highestEducation, item.relocationYN));
            }
            bac = GetSkills(bac);
            bac = GetCareers(bac);

            return bac[0];
        }

        public IEnumerable<CareerProfile> GetAllProfiles()
        {
            EmployeesEntities context = new EmployeesEntities();
            var query = (from p in context.Profiles
                         //from c in p.Careers
                         //from s in p.Skills
                         where p.country != null
                         select new
                         {
                             ProfileID = p.profileID,
                             FirstName = p.firstName,
                             LastName = p.lastName,
                             Linkedin = p.linkedinURL,
                             Portfolio = p.portfolioURL,
                             Picture = p.pictureURL,
                             City = p.city,
                             Province = p.province,
                             Country = p.country,
                             //Skills = s.skillName,
                             //Industry = c.industry,
                             //Company = c.company,
                             //JobTitle = c.jobTitle,
                             //Years = c.years,
                             HighestEduction = p.highestEducation,
                             Relocation = p.relocationYN

                         });
            List<CareerProfile> bac = new List<CareerProfile>();
            foreach (var item in query)
            {
                bac.Add(new CareerProfile(item.ProfileID, item.FirstName, item.LastName, item.Linkedin,
                    item.Portfolio, item.Picture, item.City, item.Province, item.Country, 
                    //"",
                    //item.Skills, item.Industry, item.Company, item.JobTitle, item.Years, 
                    //"", "", "", 0, 
                    item.HighestEduction, item.Relocation));
            }
            bac = GetSkills(bac);
            bac = GetCareers(bac);

            return bac;
        }


        public IEnumerable<CareerProfile> GetAllPremiumProfiles()
        {
            EmployeesEntities context = new EmployeesEntities();
            var query = (from p in context.Profiles
                         from svc in p.SvcProfiles
                         where p.profileID == svc.profileID
                         select new
                         {
                             ProfileID = p.profileID,
                             FirstName = p.firstName,
                             LastName = p.lastName,
                             Linkedin = p.linkedinURL,
                             Portfolio = p.portfolioURL,
                             Picture = p.pictureURL,
                             City = p.city,
                             Province = p.province,
                             Country = p.country,
                             HighestEduction = p.highestEducation,
                             Relocation = p.relocationYN

                         });
            List<CareerProfile> bac = new List<CareerProfile>();
            foreach (var item in query)
            {
                bac.Add(new CareerProfile(item.ProfileID, item.FirstName, item.LastName, item.Linkedin,
                    item.Portfolio, item.Picture, item.City, item.Province, item.Country,
                    item.HighestEduction, item.Relocation));
            }
            bac = GetSkills(bac);
            bac = GetCareers(bac);

            return bac;
        }


        public IEnumerable<CareerProfile> QuickSearchProfiles(string jobTitle, string city)
        {
            List<CareerProfile> bac = new List<CareerProfile>();

            IEnumerable<CareerProfile> allProfiles = GetAllProfiles();

            if(jobTitle == "" && city == "")
            {
                foreach (CareerProfile cp in allProfiles)
                {
                    return (allProfiles);
                }
                
            }
            else if (jobTitle == "" && city != "")
            {
                foreach (CareerProfile cp in allProfiles)
                {
                    if (cp.City.Trim().ToLower() == city.Trim().ToLower())
                    {
                        bac.Add(cp);
                    }
                }

            }
            else if (city == "" && jobTitle != "")
            {
                foreach (CareerProfile cp in allProfiles)
                {
                    if (cp.JobTitle.IndexOf(jobTitle, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        bac.Add(cp);
                    }
                }

            }
            else
            {
                foreach (CareerProfile cp in allProfiles)
                {
                    if (cp.City.Trim().ToLower() == city.Trim().ToLower() && cp.JobTitle.IndexOf(jobTitle, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        bac.Add(cp);
                    }
                }
            }

            return bac;
        }



        private List<CareerProfile> GetSkills(List<CareerProfile> careerProfile)
        {
            EmployeesEntities context = new EmployeesEntities();
            var query = (from p in context.Profiles
                         from s in p.Skills
                         select new
                         {
                             ProfileID = p.profileID,
                             SkillID = s.skillID,
                             SkillName = s.skillName
                         });

            List<CareerProfile> careerProfileSkill = careerProfile;
            foreach (CareerProfile cp in careerProfileSkill)
            {
                string skills = "";
                int counter = 0;

                foreach (var item in query)
                {
                    if (item.ProfileID == cp.ProfileID)
                    {
                        if (counter > 0)
                        {
                            skills += " | " + item.SkillName;
                        }
                        else
                        {
                            skills += item.SkillName;
                        }
                        counter++;
                    }
                }
                cp.Skills = skills;
            }
            return careerProfileSkill;
        }

        private List<CareerProfile> GetCareers(List<CareerProfile> careerProfile)
        {
            EmployeesEntities context = new EmployeesEntities();
            var query = (from c in context.Careers
                         //from c in p.Careers
                         select new
                         {
                             ProfileID = c.profileID,
                             Industry = c.industry,
                             Company = c.company,
                             JobTitle = c.jobTitle,
                             Years = c.years
                         });

            List<CareerProfile> careerProfileCareer = careerProfile;
            foreach (CareerProfile cp in careerProfileCareer)
            {
                string industry = "";
                string companyTitleYears = "";
                string company = "";
                string jobTitle = "";
                //int years = 0;

                int counter = 0;

                foreach (var item in query)
                {
                    if (item.ProfileID == cp.ProfileID)
                    {
                        if (counter > 0)
                        {
                            industry += "  |  " + item.Industry;
                            company += "  |  " + item.Company;
                            jobTitle += "  |  " + item.JobTitle;
                            companyTitleYears += "  |  " + item.Company + "-" + item.JobTitle + "(" + item.Years + "years)";
                            //years += item.Years;
                        }
                        else 
                        {
                            industry += item.Industry;
                            company += item.Company;
                            jobTitle += item.JobTitle;
                            companyTitleYears += item.Company + "-" + item.JobTitle + "(" + item.Years + "years)";
                        }
                        counter++;
                    }
                }
                cp.Industry = industry;
                cp.Company = company;
                cp.JobTitle = jobTitle;
                cp.CompanyTitleYears = companyTitleYears;
                //cp.Years = years;
            }
            return careerProfileCareer;
        }


    }
}