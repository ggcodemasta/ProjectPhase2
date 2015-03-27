using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class Careers
    {


        public int CareerID { get; set; }
        public int ProfileID { get; set; }
        public string Industry { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        [Required]
        [Range(1, 20)]
        public int Years { get; set; }
        public string Description { get; set; }


    }
}