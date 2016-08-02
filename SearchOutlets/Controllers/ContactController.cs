using SearchOutlets.Datastores;
using SearchOutlets.Models;
using SearchOutlets.Models.JSON;
using System.Collections.Generic;
using System.Web.Http;

namespace SearchOutlets.Controllers
{
    /// <summary>
    /// Retrieve a profile by given ID
    /// </summary>
    public class ContactController : ApiController
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
                List<Contact> allContacts = ProfileIndex.Instance.GetAllContacts();
                return Ok(allContacts);
            }
            // get the profile for the specified contact ID
            else
            {
                Contact profile = ProfileIndex.Instance.IdQuery(id);
                
                if (profile != null)
                {
                    return Ok(profile);
                }
            }

            return NotFound();
        }
    }
}
