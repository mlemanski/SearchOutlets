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
        private int id;
        private int outletId; // only 1 Outlet
        private string firstName;
        private string lastName;
        private string title;
        private string profile;


        // properties

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int OutletId
        {
            get { return outletId; }
            set { outletId = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Profile
        {
            get { return profile; }
            set { profile = value; }
        }
    }
}