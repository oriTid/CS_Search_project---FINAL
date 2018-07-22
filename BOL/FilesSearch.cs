using System;
using System.Collections.Generic;

namespace BOL
{
    public class FilesSearch
    { // This class will represent the search properties

        public int SearchID { get; set; } //will hold the searchID (taken from DB)
        public string SearchedTerm { get; set; } //will hold the search term
        public DateTime SearchDate { get; set; } //will hold the search date
        public string SearchPath { get; set; } //will hold the user initial search path (if was entered)
        public List<string> ResultsList { get; set; } //will hold the search resutls
    }
}
