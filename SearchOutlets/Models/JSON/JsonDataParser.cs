using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace SearchOutlets.Models.JSON
{
    /// <summary>
    /// Parse the Contacts and Outlets JSON files into a list or dictionary.
    /// </summary>
    class JsonDataParser<T> where T : JsonData
    {
        private const string JSON_PATH = "C:\\Users\\Matt\\Documents\\Visual Studio 2015\\Projects\\SearchOutlets\\SearchOutlets\\Datastores\\";
        private const string CONTACTS = JSON_PATH + "Contacts.json";
        private const string OUTLETS = JSON_PATH + "Outlets.json";


        /// <summary>
        /// Load the JSON file into a List of objects
        /// </summary>
        /// <returns></returns>
        public List<T> LoadProfileData()
        {
            List<T> data;

            // use the generic type to determine which json file to parse
            string json_path = typeof(T) == typeof(Contact) ? CONTACTS : OUTLETS;

            using (StreamReader sr = new StreamReader(json_path))
            {
                string json = sr.ReadToEnd();
                data = JsonConvert.DeserializeObject<List<T>>(json);
            }

            return data;
        }


        /// <summary>
        /// Load the JSON file into a dictionary of ID-to-object mappings
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, T> LoadProfileDataMap()
        {
            Dictionary<int, T> dataMap = new Dictionary<int, T>();
            foreach (T item in LoadProfileData())
            {
                dataMap.Add(item.Id, item);
            }

            return dataMap;
        }
    }
}
