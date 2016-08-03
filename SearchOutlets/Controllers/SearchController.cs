using SearchOutlets.Datastores;
using SearchOutlets.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SearchOutlets.Controllers
{
    /// <summary>
    /// Execute a search based on the items given in the text field
    /// </summary>
    public class SearchController : ApiController
    {
        /// <summary>
        /// Perform a string query on all fields in the contact's profile
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Search(string query = "")
        {
            if (query.Length > 0)
            {
                List<Contact> contacts = ProfileIndex.Instance.FullSearch(query);
                return Ok(contacts);
            }

            return Ok();
        }
    }
}
