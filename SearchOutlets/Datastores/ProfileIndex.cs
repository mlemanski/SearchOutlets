using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
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

            Dictionary<int, Outlet> outlets = new JsonDataParser<Outlet>().LoadProfileDataMap();

            foreach (Contact contact in new JsonDataParser<Contact>().LoadProfileData())
            {
                Document d = new Document();
                d.Add(new Field("id", contact.Id.ToString(), Field.Store.YES, Field.Index.NO));
                d.Add(new Field("outletId", contact.OutletId.ToString(), Field.Store.NO, Field.Index.NO));
                d.Add(new Field("outlet", outlets[contact.OutletId].Name, Field.Store.YES, Field.Index.ANALYZED));
                d.Add(new Field("firstName", contact.FirstName, Field.Store.YES, Field.Index.NOT_ANALYZED));
                d.Add(new Field("lastName", contact.LastName, Field.Store.YES, Field.Index.NOT_ANALYZED));
                d.Add(new Field("title", contact.Title, Field.Store.YES, Field.Index.ANALYZED));
                d.Add(new Field("profile", contact.Profile, Field.Store.YES, Field.Index.ANALYZED));
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
            directory.ListAll();
        }
    }
}