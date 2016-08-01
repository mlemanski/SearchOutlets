using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchOutlets.Models
{
    /// <summary>
    /// Internal representation of an item from the Outlets.json file
    /// </summary>
    public class Outlet
    {
        private int Id { get; set; }
        private string Name { get; set; }
    }
}