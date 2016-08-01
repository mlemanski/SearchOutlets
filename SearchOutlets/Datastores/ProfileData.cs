using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Collections;

namespace SearchOutlets.Datastores
{
    public class ProfileData
    {
        private Dictionary<int, Models.Contact> Profiles { get; set; }

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
                Profiles.Add(contact.Id(), contact);
            }
        }
    }
}