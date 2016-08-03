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
        private const int ALL_CONTACTS = -1;

        /// <summary>
        /// Retrieve the Contact matching the given identifier, if any such Contact exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Contact(int id = ALL_CONTACTS)
        {
            // get all contacts
            if (id == ALL_CONTACTS)
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
