using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchOutlets.Indexes
{
    /// <summary>
    /// This class controls all access to the profile index.
    /// 
    /// This exercise contains a pretty small amount of text data, so it
    /// should be fine to just keep in memory. If there was more data we
    /// could use the file system-based index.
    /// </summary>
    public class ProfileIndex
    {
        private static ProfileIndex instance;

        // private constructor to support the singleton model
        private ProfileIndex() { }

        // return the single instance of ProfileIndex, initializing if necessary
        public static ProfileIndex Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProfileIndex();
                }

                return instance;
            }
        }



        // the index storing all profile data
        private Directory index = new RAMDirectory();

    }
}