using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchOutlets.Models
{
    /// <summary>
    /// A contact's profile data that will displayed in the GUI. Different
    /// from Contact, Outlet, and JsonData in that those are representations
    /// of the associated JSON objects.
    /// 
    /// This class will be used for interacting with the index.
    /// </summary>
    public class Contact
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Outlet { get; set; }
        public string Profile { get; set; }
    }
}