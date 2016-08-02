using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SearchOutlets.Models;
using SearchOutlets.Models.JSON;
using System.Collections.Generic;

namespace SearchOutlets.Datastores
{
    /// <summary>
    /// This class wraps the backing index file, analyzer, and other 
    /// associated files.
    /// </summary>
    public class ProfileIndex
    {
        private static class ContactField
        {
            public static string ID = "id";
            public static string OUTLET_ID = "outletId";
            public static string OUTLET = "outlet";
            public static string FIRST_NAME = "firstName";
            public static string LAST_NAME = "lastName";
            public static string TITLE = "title";
            public static string PROFILE = "profile";
        }


        // the single instance
        private static ProfileIndex instance;


        // Lucene index objects
        private Directory directory;

        // private constructor to support the singleton model
        private ProfileIndex()
        {
            // the actual file storing all profile data
            directory = FSDirectory.Open("ContactIndex");
        }

        // return the single instance of ProfileIndex, initializing if necessary
        public static ProfileIndex Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProfileIndex();
                    instance.InitializeIndex();
                }

                return instance;
            }
        }


        /// <summary>
        /// Load the Contacts and Outlets data into the index
        /// </summary>
        public void InitializeIndex()
        {
            // objects for building the index
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexWriter writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

            Dictionary<int, JsonOutlet> outlets = new JsonDataParser<JsonOutlet>().LoadProfileDataMap();

            foreach (JsonContact contact in new JsonDataParser<JsonContact>().LoadProfileData())
            {
                Document d = new Document();
                d.Add(new Field(ContactField.ID,         contact.Id.ToString(),          Field.Store.YES, Field.Index.NO));
                d.Add(new Field(ContactField.OUTLET_ID,  contact.OutletId.ToString(),    Field.Store.NO,  Field.Index.NO));
                d.Add(new Field(ContactField.OUTLET,     outlets[contact.OutletId].Name, Field.Store.YES, Field.Index.ANALYZED));
                d.Add(new Field(ContactField.FIRST_NAME, contact.FirstName,              Field.Store.YES, Field.Index.NOT_ANALYZED));
                d.Add(new Field(ContactField.LAST_NAME,  contact.LastName,               Field.Store.YES, Field.Index.NOT_ANALYZED));
                d.Add(new Field(ContactField.TITLE,      contact.Title,                  Field.Store.YES, Field.Index.ANALYZED));
                d.Add(new Field(ContactField.PROFILE,    contact.Profile,                Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(d);
            }

            writer.Optimize();
            writer.Flush(true, true, true);
            writer.Dispose();
        }



        /// <summary>
        /// Retrieve all contacts from the index
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetAllContacts()
        {
            IndexSearcher searcher = new IndexSearcher(directory, true); // true means read-only
            MatchAllDocsQuery queryAll = new MatchAllDocsQuery();
            TopScoreDocCollector collector = TopScoreDocCollector.Create(100, false); // top 100 results, unsorted
            ScoreDoc[] scoredDocs = collector.TopDocs().ScoreDocs;

            // convert the top results to Contact objects, to be displayed at the GUI
            List<Contact> results = new List<Contact>(scoredDocs.Length);
            foreach (ScoreDoc scoreDoc in scoredDocs)
            {
                Document doc = searcher.Doc(scoreDoc.Doc);
                Contact contact = new Contact();
                contact.Name = doc.Get(ContactField.FIRST_NAME) + " " + doc.Get(ContactField.LAST_NAME);
                contact.Title = doc.Get(ContactField.TITLE);
                contact.Outlet = doc.Get(ContactField.OUTLET);
                contact.Profile = doc.Get(ContactField.PROFILE);
                results.Add(contact);
            }

            return results;
        }
    }
}