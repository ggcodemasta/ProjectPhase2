//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Skill
    {
        public Skill()
        {
            this.Profiles = new HashSet<Profile>();
        }
    
        public int skillID { get; set; }
        public string category { get; set; }
        public string skillName { get; set; }
        public string description { get; set; }
    
        public virtual ICollection<Profile> Profiles { get; set; }
    }
}
