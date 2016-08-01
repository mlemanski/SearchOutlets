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
        /// Retrieve the Contact matching the given identifier, if any such Contact exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Profile(int id)
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
