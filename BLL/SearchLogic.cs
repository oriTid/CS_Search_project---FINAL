using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DAL;

namespace BLL
{
    public class SearchLogic : FilesSearch
    {
        public event Action<string> ResultFoundEvent; //generate event to invoke when there is a result
        public event Action<string> ShowProgress; //generate event to show progress of search
        public SearchLogic() //ctor
        {
            DBmanager.myDataConnctionString = "Data Source=devps2010;Initial Catalog=CS_Search_Project_Ori;Integrated Security=True"; //set data connection string in DAL from the ctor
        }

        #region  Search Functions
        public void MainSearchFunc(string userSearchTerm, string userSearchedPath) //main search function. gets the search term + search path
        {
            int rowsAffected = 0;
            ResultsList = new List<string>(); // This will hold *all the results of the search"
            List<string> dirs = new List<string>(); //This will hold *all*  Top and Subfloders that program can search in

            if (userSearchedPath != "") //when search path was input by user
            {
                dirs = GetDirectoriesList(userSearchedPath); //populate "dirs" list with GetDirectoriesList func

            }
            else   //when search path was empty
            {
                foreach (DriveInfo inf in DriveInfo.GetDrives().Where(x => x.IsReady && x.DriveType == DriveType.Fixed)) //get local drives on computer. Use the IsReady property to test whether a drive is ready because using this method on a drive that is not ready will throw a IOException.
                {
                    dirs.AddRange(GetDirectoriesList(inf.RootDirectory.FullName)); //populate "dirs" list with GetDirectoriesList func
                }

            }

            ResultFoundEvent?.Invoke($"Found {dirs.Count} directories to search in.");
            ResultFoundEvent?.Invoke("");
            ResultFoundEvent?.Invoke("Results:");

            foreach (string folder in dirs)
            {
                try
                {
                    List<string> res = Directory.GetFiles(folder, "*" + userSearchTerm + "*.*").ToList();
                    res.ForEach(r => ResultFoundEvent?.Invoke(r)); //create event when a file was found
                    ResultsList.AddRange(res); //add all the search resutls from each folder to the Results list

                }
                catch { }
            }

            SearchedTerm = userSearchTerm; //for update the DB later

            rowsAffected = DBmanager.UpdateDbResults(this); //insert results to DB and updating back number of rows affected in DB

            if (rowsAffected <= 1) //if now rows affected, there were no results. raise event
            {
                ResultFoundEvent?.Invoke("");
                ResultFoundEvent?.Invoke("No results found");
            }
            else
            {
                ResultFoundEvent?.Invoke("");
                ResultFoundEvent?.Invoke($"\n{dirs.Count} directories were scanned.\n{rowsAffected - 1} results were found for the searched term '{SearchedTerm}'.\nDB was updated susccesfully.\n"); //raise event to notify how many rows were affected (because 2 tables are always affected, I minus -1 to count)
            }
        }

        private List<string> GetDirectoriesList(string searchPath) //search in all subfolders in the given path
        {
            List<string> dirs = new List<string>();

            dirs.Add(searchPath);
            foreach (string folder in Directory.GetDirectories(searchPath, "*", SearchOption.TopDirectoryOnly).ToArray()) //loop on all top directoires
            {
                dirs.Add(folder); // add this folder to the dirs list
                SearchSubDirectoriesRecursive(folder, dirs); //send each top directory to the recursive search func               
            }

            return dirs;
        }

        private void SearchSubDirectoriesRecursive(string subFolder, List<string> dirs) //recrusive func to loop on al sub-folders that were sent to it
        {
            try
            {
                ShowProgress?.Invoke("");
                foreach (string folder in Directory.GetDirectories(subFolder, "*", SearchOption.TopDirectoryOnly).ToArray()) //loop on all top directoires
                {
                    dirs.Add(folder); // add this folder to the dirs list
                    SearchSubDirectoriesRecursive(folder, dirs);//send each directory to the search recursive func again - recursive

                }
            }
            catch { }
        }
        #endregion
        public List<string> ReturnDBData() => DBmanager.ShowDBData(); //function to return all DB data

    }
}
