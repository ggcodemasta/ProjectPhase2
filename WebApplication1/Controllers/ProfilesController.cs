using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1;
using WebApplication1.ViewModels;
using WebApplication1.Models;
using System.Web.Http.Cors;

namespace WebApplication1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfilesController : ApiController
    {
        private EmployeesEntities db = new EmployeesEntities();

        // GET: api/Profiles
        public IEnumerable<CareerProfile> GetProfiles()
        {
            CareerProfileRepository cpr = new CareerProfileRepository();
            IEnumerable<CareerProfile> profileList = cpr.APIGetAllProfiles();

            return profileList;
        }

        // GET: api/Profiles/5
        [ResponseType(typeof(Profile))]
        public IHttpActionResult GetProfile(int id)
        {
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }


        // GET: api/Profiles/jobtitle/string   
        public IEnumerable<CareerProfile> GetProfiles(string jobTitle, string city)
        {
            if(jobTitle == "null")
            {
                jobTitle = "";
            }
            if (city == "null")
            {
                city = "";
            }
            CareerProfileRepository cpr = new CareerProfileRepository();
            IEnumerable<CareerProfile> profileList = cpr.QuickSearchProfiles(jobTitle, city);

            return profileList;
        }


        //THIS WORKS
        // GET: api/Profiles/city
        //public IEnumerable<CareerProfile> GetProfilesCity(string city)
        //{
        //    CareerProfileRepository cpr = new CareerProfileRepository();
        //    IEnumerable<CareerProfile> profileList = cpr.QuickSearchProfiles("", city);

        //    return profileList;
        //}









        // GET: api/Profiles/5
        //[ResponseType(typeof(Profile))]
        //public IHttpActionResult GetProfile(int id)
        //{
        //    Profile profile = db.Profiles.Find(id);
        //    if (profile == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(profile);
        //}

        // PUT: api/Profiles/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutProfile(int id, Profile profile)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != profile.profileID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(profile).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProfileExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Profiles
        //[ResponseType(typeof(Profile))]
        //public IHttpActionResult PostProfile(Profile profile)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Profiles.Add(profile);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = profile.profileID }, profile);
        //}

        //// DELETE: api/Profiles/5
        //[ResponseType(typeof(Profile))]
        //public IHttpActionResult DeleteProfile(int id)
        //{
        //    Profile profile = db.Profiles.Find(id);
        //    if (profile == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Profiles.Remove(profile);
        //    db.SaveChanges();

        //    return Ok(profile);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ProfileExists(int id)
        //{
        //    return db.Profiles.Count(e => e.profileID == id) > 0;
        //}
    }
}