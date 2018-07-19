using System;
using System.Collections.Generic;

namespace BOL
{
    public class FilesSearch
    { // This class will represent the search properties

        public int SearchID { get; set; }
        public string SearchedTerm { get; set; }
        public DateTime SearchDate { get; set; }
        public string SearchPath { get; set; }
        public List<string> ResultsList { get; set; }
    }
}
