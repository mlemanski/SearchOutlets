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
        //private int id;
        private string name;


        // properties
                //public int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}


        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}