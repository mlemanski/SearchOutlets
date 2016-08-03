using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SearchOutlets.Controllers
{
    public class InitializeIndexingController : ApiController
    {
        /// <summary>
        /// Indexing takes about 10 seconds to do, which makes that initial 
        /// request take a misleadingly long amount of time. So, start 
        /// initializing as soon as the web page is loaded.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult InitializeIndexing()
        {
            // making a reference to the singleton instance will invoke initialization
            if (Datastores.ProfileIndex.Instance != null)
                System.Diagnostics.Debug.WriteLine("Pre-initializing index");

            return Ok();
        }
    }
}
