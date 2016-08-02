using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchOutlets.Models
{
    /// <summary>
    /// An abstract parent class to avoid duplicating JSON parsing code.
    /// </summary>
    public abstract class ProfileData
    {
        protected int id;

        public int Id
        {
            get;
            set;
        }
    }
}
