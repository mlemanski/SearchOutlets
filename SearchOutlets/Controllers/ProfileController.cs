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
        public List<Models.Contact> GetAllContacts()
        {
            return new List<Models.Contact>(Datastores.ProfileDatastore.Instance.ProfileData.Values);
        }
    }
}
