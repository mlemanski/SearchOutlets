using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
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
        private Directory directory;
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

        }
    }
}