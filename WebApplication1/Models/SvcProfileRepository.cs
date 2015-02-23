using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SvcProfileRepository
    {
        static int AddSvcProfile(SvcProfile svcProfile)
        {
            EmployeesEntities db = new EmployeesEntities();
            db.SvcProfiles.Add(svcProfile);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return -1;
            }
            return 1;

        }

        public int HandleAddingSvcProfile(int svcTypeID, string billingMethod, string email="")
        {
            ProfileRepository profileRepository = new ProfileRepository();
        
            if (string.IsNullOrWhiteSpace(email))
            {
                email = HttpContext.Current.User.Identity.Name;
            }
            int profileID = profileRepository.GetProfileIDByEmail(email);

            SvcTypeRepository svcTypeRepository = new SvcTypeRepository();
            SvcType svcType = svcTypeRepository.GetOneSvcType(svcTypeID);

            SvcProfile svcProfile = new SvcProfile();
            svcProfile.profileID = profileID;
            svcProfile.svcTypeID = svcTypeID;
            svcProfile.billingDate = DateTime.Now;
            svcProfile.billingCode = "temporary";
            if (billingMethod == "")
            {
                billingMethod = "Credit Card";
            }
            svcProfile.billingMethod = billingMethod;
            svcProfile.svcStartDate = DateTime.Now;
            svcProfile.svcEndDate = DateTime.Now.AddDays(svcType.svcWeeks * 7);

            return AddSvcProfile(svcProfile); 
        }

        public IEnumerable<SvcProfile> GetManySvcProfilesByProfileID(int profileID)
        {
            EmployeesEntities db = new EmployeesEntities();
            return db.SvcProfiles.Where(r => r.profileID == profileID)
                            .OrderByDescending(r => r.svcProfileID).ToList();
        }

        public IEnumerable<SvcProfile> HandleGettingManySvcProfilesByProfileID()
        {
            ProfileRepository profileRepository = new ProfileRepository();
            int profileID = profileRepository.GetProfileIDByEmail(HttpContext.Current.User.Identity.Name);
            return GetManySvcProfilesByProfileID(profileID);
        }

    }
}