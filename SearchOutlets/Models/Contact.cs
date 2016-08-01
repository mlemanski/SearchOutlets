using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchOutlets.Models
{
    /// <summary>
    /// Internal representation of an item from the Contacts.json file
    /// </summary>
    public class Contact
    {
        private int Id { get; set; }
        private int OutletId { get; set; } // only 1 Outlet
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Title { get; set; }
        private string Profile { get; set; }
    }
}