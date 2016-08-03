using Newtonsoft.Json;
using SearchOutlets.Datastores;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace SearchOutlets.Models.JSON
{
    /// <summary>
    /// Parse the Contacts and Outlets JSON files into a list or dictionary.
    /// </summary>
    class JsonDataParser<T> where T : JsonData
    {
        private static readonly string JSON_PATH = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
        private static readonly string CONTACTS = Path.Combine(JSON_PATH, "Contacts.json");
        private static readonly string OUTLETS = Path.Combine(JSON_PATH, "Outlets.json");


        /// <summary>
        /// Load the JSON file into a List of objects
        /// </summary>
        /// <returns></returns>
        public List<T> LoadProfileData()
        {
            List<T> data;

            // use the generic type to determine which json file to parse
            string json_path = typeof(T) == typeof(JsonContact) ? CONTACTS : OUTLETS;

            StreamReader sr = new StreamReader(json_path);
            data = JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());

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
