using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Newtonsoft.Json;
using SearchOutlets.Datastores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SearchOutlets.Indexes
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
        private Lucene.Net.Store.Directory directory;
        private Analyzer analyzer;
        private IndexWriter writer;

        // private constructor to support the singleton model
        private ProfileIndex()
        {
            // the actual file storing all profile data
            directory = FSDirectory.Open("ProfileIndex.db");

            analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        // return the single instance of ProfileIndex, initializing if necessary
        public static ProfileIndex Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProfileIndex();
                    instance.LoadProfiles();
                }

                return instance;
            }
        }


        /// <summary>
        /// Load the Contacts and Outlets data into the index
        /// </summary>
        public void LoadProfiles()
        {
            Dictionary<int, Models.Outlet> outlets = new Models.ProfileDataParser<Models.Outlet>().LoadProfileDataMap();

            foreach (Models.Contact contact in new Models.ProfileDataParser<Models.Contact>().LoadProfileData())
            {
                Document d = new Document();
                d.Add(new Field("id", contact.Id.ToString(), Field.Store.NO, Field.Index.NO));
                d.Add(new Field("outletId", contact.OutletId.ToString(), Field.Store.NO, Field.Index.NO));
                d.Add(new Field("outlet", outlets[contact.OutletId].Name, Field.Store.YES, Field.Index.ANALYZED));
                d.Add(new Field("firstName", contact.FirstName, Field.Store.YES, Field.Index.NOT_ANALYZED));
                d.Add(new Field("lastName", contact.LastName, Field.Store.YES, Field.Index.NOT_ANALYZED));
                d.Add(new Field("title", contact.Title, Field.Store.YES, Field.Index.ANALYZED));
                d.Add(new Field("profile", contact.Profile, Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(d);
            }
        }
    }
}