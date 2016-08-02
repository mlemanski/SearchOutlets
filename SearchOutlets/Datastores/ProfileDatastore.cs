﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Collections;

namespace SearchOutlets.Datastores
{
    /// <summary>
    /// Represents the profile datastore.
    /// 
    /// "In real life" this would be an index, database, etc.
    /// </summary>
    public class ProfileDatastore
    {
        private static ProfileDatastore instance;

        // private constructor to support the singleton model
        private ProfileDatastore() { }

        // return the single instance of ProfileDatastore, initializing if necessary
        public static ProfileDatastore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProfileDatastore();
                    instance.Load();
                }

                return instance;
            }
        }



        public Dictionary<int, Models.Contact> ProfileData { get; set; }

        /// <summary>
        /// Read the Contacts.json file into an in-memory datastore
        /// </summary>
        public void Load()
        {
            List<Models.Contact> contacts;

            using (StreamReader sr = new StreamReader("Contacts.json"))
            {
                string json = sr.ReadToEnd();
                contacts = JsonConvert.DeserializeObject<List<Models.Contact>>(json);
            }

            foreach (Models.Contact contact in contacts)
            {
                ProfileData.Add(contact.Id, contact);
            }
        }
    }
}