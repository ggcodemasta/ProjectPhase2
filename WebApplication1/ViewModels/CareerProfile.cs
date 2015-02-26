using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class CareerProfile
    {
        public int ProfileID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LinkedinURL { get; set; }
        public string PortfolioURL { get; set; }

        public string PictureURL { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Skills { get; set; }
        public string Industry { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public int Years { get; set; }
        public string HighestEducation { get; set; }
        public string Relocation { get; set; }
        public string CompanyTitleYears { get; set; }

        public CareerProfile() { }

        public CareerProfile(int profileID, string firstName, string lastName, string linkedin,
            string portfolio, string picture, string city, string province, string country, string skills,
            string industry, string company, string jobTitle, int years, string highestEduction, string relocation)
        {
            ProfileID = profileID;
            FirstName = firstName;
            LastName = lastName;
            LinkedinURL = linkedin;
            PortfolioURL = portfolio;
            PictureURL = picture;
            City = city;
            Province = province;
            Country = country;
            Skills = skills;
            Industry = industry;
            Company = company;
            JobTitle = jobTitle;
            Years = years;
            HighestEducation = highestEduction;
            Relocation = relocation;
        }

        public CareerProfile(int profileID, string firstName, string lastName, string linkedin,
            string portfolio, string picture, string city, string province, string country,
            string highestEduction, string relocation)
        {
            ProfileID = profileID;
            FirstName = firstName;
            LastName = lastName;
            LinkedinURL = linkedin;
            PortfolioURL = portfolio;
            PictureURL = picture;
            City = city;
            Province = province;
            Country = country;
            HighestEducation = highestEduction;
            Relocation = relocation;
        }

        public CareerProfile(int profileID, string firstName, string lastName, string city, string province, string country)
        {
            ProfileID = profileID;
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Province = province;
            Country = country;
        }



    }
}