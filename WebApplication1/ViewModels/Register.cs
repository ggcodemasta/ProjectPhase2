using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 


namespace WebApplication1.ViewModels
{
    public class Register
    {
        public int ProfileID { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        ErrorMessage = "This is not a valid email address.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
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