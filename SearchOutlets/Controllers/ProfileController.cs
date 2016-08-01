using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SearchOutlets.Controllers
{
    /// <summary>
    /// Retrieve a profile by given ID
    /// </summary>
    public class ProfileController : ApiController
    {
        /// <summary>
        /// Retrieve all Contacts from the in-memory datastore -- no searching needed
        /// </summary>
        /// <returns></returns>
        public List<Models.Contact> GetAllProfiles()
        {
            return new List<Models.Contact>(Datastores.ProfileDatastore.Instance.ProfileData.Values);
        }


        /// <summary>
        /// Retrieve the Contact matching the given identifier, if any such Contact exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult GetProfile(int id)
        {
            Models.Contact profile = null;
            Datastores.ProfileDatastore.Instance.ProfileData.TryGetValue(id, out profile);

            if (profile != null)
            {
                return Ok(profile);
            }

            return NotFound();
        }
    }
}
