//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Career
    {
        public int careerID { get; set; }
        public int profileID { get; set; }
        public string industry { get; set; }
        public string company { get; set; }
        public string jobTitle { get; set; }
        public int years { get; set; }
        public string description { get; set; }
    
        public virtual Profile Profile { get; set; }
        public virtual Industry Industry1 { get; set; }
        public virtual Jobtitle Jobtitle1 { get; set; }
    }
}
