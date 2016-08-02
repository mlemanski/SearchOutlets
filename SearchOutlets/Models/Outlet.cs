using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchOutlets.Models
{
    /// <summary>
    /// Internal representation of an item from the Outlets.json file
    /// </summary>
    public class Outlet : ProfileData
    {
        public string Name { get; set; }
    }
}