using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BOL;

namespace DAL
{
    public static class DBmanager
    {
        public static string myDataConnctionString; //will be set each search from the BLL

        #region DB Update
        public static int UpdateDbResults(FilesSearch filesearch)
        {
            int rowsAffected = 0; //affected rows
            int searchID = 0;
            try
            {
                using (SqlConnection mySqlConnection = new SqlConnection(myDataConnctionString))
                {
                    mySqlConnection.Open();
                    SqlCommand query = new SqlCommand($"INSERT INTO [dbo].[Searches] ([SearchTerm],[SearchDate]) VALUES ('{filesearch.SearchedTerm}',getdate()) ", mySqlConnection);
                    rowsAffected = query.ExecuteNonQuery();
                    query = new SqlCommand("select @@IDENTITY as ID", mySqlConnection);//get the last searchID from DB
                    searchID = int.Parse(query.ExecuteScalar().ToString());


                    foreach (string path in filesearch.ResultsList) //inserting each search result (ID+Path) to the search_results DB
                    {
                        query = new SqlCommand($"INSERT INTO [dbo].[Search_Results] ([SearchID],[SearchResultPath]) VALUES ({searchID},N'{path.Replace("'","''")}')", mySqlConnection);
                        rowsAffected += query.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return rowsAffected; //Sum of all rows affected in both DB tables

        }
        #endregion

        #region Show DB Data
        public static List<string> ShowDBData()
        {
            SqlDataReader reader = null; // generating reader prop to get the DB whole Data
            List<string> dbData = new List<string>(); //generating list to add the data to

            using (SqlConnection mySqlConnection = new SqlConnection(myDataConnctionString))
            {
                mySqlConnection.Open();
                SqlCommand query = new SqlCommand("SELECT s.SearchID, s.SearchTerm, s.SearchDate, sr.SearchResultPath FROM DBO.Searches AS s LEFT JOIN DBO.Search_Results AS sr ON s.SearchID = sr.SearchID", mySqlConnection);
                reader = query.ExecuteReader();

                while (reader.Read()) //generate the joined data sting from the query
                {
                    dbData.Add($"ID: {reader[0]} Search Term: {reader[1]} Search Date: {DateTime.Parse(reader[2].ToString()).ToString("dd/MM/yyyy HH:mm:ss") } Result Path: {reader[3]}");
                }
            }
                        
            return dbData; // return the dbData list


        }
        #endregion
    }
}

