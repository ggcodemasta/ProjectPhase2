using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 


namespace WebApplication1.ViewModels
{
    public class ProfileViewModel
    {
        public int ProfileID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LinkedinURL { get; set; }
        public string PictureURL { get; set; }
        public string PortfolioURL { get; set; }
        public string HighestEducation { get; set; }
        public string RelocationYN { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set;  }
    }
}