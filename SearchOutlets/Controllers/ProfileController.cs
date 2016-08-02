using SearchOutlets.Models.JSON;
using System.Collections.Generic;
using System.Web.Http;

namespace SearchOutlets.Controllers
{
    /// <summary>
    /// Retrieve a profile by given ID
    /// </summary>
    public class ProfileController : ApiController
    {
        private const int ALL_PROFILES = -1;

        /// <summary>
        /// Retrieve the Contact matching the given identifier, if any such Contact exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Profile(int id = ALL_PROFILES)
        {
            // get all profiles
            if (id == ALL_PROFILES)
            {
                List<Contact> allProfiles = new JsonDataParser<Contact>().LoadProfileData();
                return Ok(allProfiles);
            }
            // get the profile for the specified contact ID
            else
            {
                Contact profile = null;
                new JsonDataParser<Contact>().LoadProfileDataMap().TryGetValue(id, out profile);
                
                if (profile != null)
                {
                    return Ok(profile);
                }
            }

            return NotFound();
        }
    }
}
