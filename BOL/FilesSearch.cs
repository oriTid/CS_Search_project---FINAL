using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class FilesSearch
    { // This class will represente the search properties

        public int SearchID { get; set; }
        public string SearchedTerm { get; set; }
        public DateTime SearchDate { get; set; }
        public string SearchPath { get; set; }
        public List<string> ResultsList { get; set; }
    }
}
