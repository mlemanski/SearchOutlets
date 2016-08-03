using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SearchOutlets.Models;
using SearchOutlets.Models.JSON;
using System.Collections.Generic;

namespace SearchOutlets.Datastores
{
    /// <summary>
    /// This class provides access to the Lucene index API
    /// </summary>
    public class ProfileIndex
    {
        public const string INDEX_NAME = "\\SearchOutletsIndex";

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


        // Lucene index object
        private Directory directory;
        private Analyzer analyzer;

        // private constructor to support the singleton model
        private ProfileIndex()
        {
            // the actual file storing all profile data
            directory = FSDirectory.Open(INDEX_NAME);
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
            analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexWriter writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);

            Dictionary<int, JsonOutlet> outlets = new JsonDataParser<JsonOutlet>().LoadProfileDataMap();

            foreach (JsonContact contact in new JsonDataParser<JsonContact>().LoadProfileData())
            {
                Document d = new Document();
                d.Add(new Field(ContactField.ID,         contact.Id.ToString(),          Field.Store.YES, Field.Index.NOT_ANALYZED));
                d.Add(new Field(ContactField.OUTLET_ID,  contact.OutletId.ToString(),    Field.Store.YES, Field.Index.NO));
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
            return RunQuery(new MatchAllDocsQuery(), false);
        }


        /// <summary>
        /// Try to find a profile matching the exact user ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contact IdQuery(int id)
        {
            TermQuery tq = new TermQuery(new Term(ContactField.ID, id.ToString()));

            List<Contact> results = RunQuery(tq, true);
            if (results.Count > 0)
            {
                return results[0];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Retrieve all contacts from the index matching the given name query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Contact> ContactSearch(string query)
        {
            // match first and last name fields
            MultiFieldQueryParser qp = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,
                                                                 new string[]{ ContactField.FIRST_NAME, ContactField.LAST_NAME },
                                                                 analyzer);
            Query q = qp.Parse(query);
            return RunQuery(q, true);
        }



        /// <summary>
        /// Search across all searchable fields to identify contacts that best match the given query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Contact> FullSearch(string query)
        {
            MultiFieldQueryParser qp = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,
                                                                 new string[]
                                                                 {
                                                                     ContactField.FIRST_NAME,
                                                                     ContactField.LAST_NAME,
                                                                     ContactField.TITLE,
                                                                     ContactField.OUTLET,
                                                                     ContactField.PROFILE
                                                                 },
                                                                 analyzer);
            Query q = qp.Parse(query);
            return RunQuery(q, true);
        }


        /// <summary>
        /// Run a query against the index
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Contact> RunQuery(Query query, bool sortResults)
        {
            IndexSearcher searcher = new IndexSearcher(directory, true); // true means read-only
            TopScoreDocCollector collector = TopScoreDocCollector.Create(100, sortResults); // top 100 results
            searcher.Search(query, collector);

            ScoreDoc[] scoredDocs = collector.TopDocs().ScoreDocs;

            // convert the top results to Contact objects, to be displayed at the GUI
            List<Contact> results = ScoreDocsToContacts(searcher, scoredDocs);

            searcher.Dispose();

            return results;
        }



        /// <summary>
        /// Convert the scored results into a list of Contacts
        /// </summary>
        /// <param name="searcher"></param>
        /// <param name="scoredDocs"></param>
        /// <returns></returns>
        private List<Contact> ScoreDocsToContacts(IndexSearcher searcher, ScoreDoc[] scoredDocs)
        {
            List<Contact> contacts = new List<Contact>(scoredDocs.Length);

            foreach (ScoreDoc scoreDoc in scoredDocs)
            {
                Document doc = searcher.Doc(scoreDoc.Doc);
                Contact contact = this.DocumentToContact(doc);
                contacts.Add(contact);
            }

            return contacts;
        }

        /// <summary>
        /// Use data from a Document to create a new Contact
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private Contact DocumentToContact(Document doc)
        {
            Contact contact = new Contact();
            contact.Name = doc.Get(ContactField.FIRST_NAME) + " " + doc.Get(ContactField.LAST_NAME);
            contact.Title = doc.Get(ContactField.TITLE);
            contact.Outlet = doc.Get(ContactField.OUTLET);
            contact.Profile = doc.Get(ContactField.PROFILE);
            return contact;
        }
    }
}