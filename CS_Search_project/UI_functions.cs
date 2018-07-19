using System;
using System.Collections.Generic;
using BLL;

namespace UIL
{
    public static class UI_functions
    {
        private static int ProgressCnt = 0; // is used in the progress event (for the switch case)
        #region Startup Func
        public static int Startup()
        {
            ConsoleKeyInfo? userInput = null; // value that gets the key input
            int userChoice = -1; // value for parsing the user input
            string userSearchTerm = "";
            string userSearchedPath = "";

            SearchLogic newSearch = new SearchLogic(); //start a new search after all conditions are filled
            newSearch.ResultFoundEvent += NewSearch_ResultFoundEvent; // register to the results found event
            newSearch.ShowProgress += NewSearch_ShowProgress; // register to the progres event

            Console.Clear();
            Console.WriteLine("Welcome to Ori's files search engine.");
            Console.WriteLine();
            Console.WriteLine("*** Note - the search engine will not search protected folders ***");
            Console.WriteLine();
            Console.WriteLine("Please choose an operation:");
            Console.WriteLine();
            Console.WriteLine("1. Search all computer -  NOTE:This search may take long time");
            Console.WriteLine("2. Search in a known directory and its sub-directories");
            Console.WriteLine("3. Show all searches in DB");
            Console.WriteLine("4. Exit");
            Console.WriteLine();
            Console.WriteLine();

            while (userChoice < 1 || userChoice > 4) //make sure user choice is from 1-4
            {
                if (userInput != null)
                {
                    Console.WriteLine("Please choose operation 1-4");
                }

                userInput = System.Console.ReadKey(true); //gets the key input
                int.TryParse(userInput?.KeyChar.ToString(), out userChoice); //parse the key input to a number
            }

            switch (userChoice)
            {
                case 1:

                    userSearchTerm = GetUserTerm();
                    break;

                case 2:
                    bool beenHere = false; //flag for the path retry message

                    userSearchTerm = GetUserTerm();

                    Console.WriteLine("Please enter path to search:");
                    while (!System.IO.Directory.Exists(userSearchedPath)) //if the path does not exists
                    {
                        if (beenHere)
                            Console.WriteLine("Directory not found. Please use a valid path.");

                        userSearchedPath = Console.ReadLine(); //gets the path from user
                        beenHere = true;
                    }
                    break;

                case 3:
                    List<string> CurrentDbData = newSearch.ReturnDBData(); // new list to get the current DB searches and results
                    if (CurrentDbData.Count > 0)
                        CurrentDbData.ForEach(row => Console.WriteLine(row)); //run the get DBData func and print the rows to console
                    else
                        Console.WriteLine("There is no data in the DB");

                    return 0;

                case 4: return 4; //exit
            }

            Console.WriteLine($"Please wait while Im searching:'{userSearchTerm}' .........");
            Console.WriteLine();
            newSearch.MainSearchFunc(userSearchTerm, userSearchedPath); //send the search term + path to the main search function

            return 0;
        }
        #endregion
        private static string GetUserTerm() //validate the user search term
        {
            string userSearchTerm = "";
            Console.WriteLine("Please enter a term to search:");
            while (!ValidateInputChar(userSearchTerm)) //send user input to validate option
            {
                userSearchTerm = Console.ReadLine();
                if (userSearchTerm.Trim() == "") //verify that the searchterm isnt equal to space
                    Console.WriteLine("You cant search only for 'space' or 'enter' in the search term. try again");
            }
            return userSearchTerm;
        }
        private static bool ValidateInputChar(string userInput) => userInput.Trim() != ""; //user input validation func
        private static void NewSearch_ResultFoundEvent(string eventAnswer) 
        {
            Console.WriteLine(eventAnswer); //print event answers (strings) in console
        }
        private static void NewSearch_ShowProgress(string eventAnswer) //print symbol to screen (progress) while searching directories
        {             
            switch (ProgressCnt)
            {
                case 0: Console.Write("\b|"); break;
                case 1: Console.Write("\b/"); break;
                case 2: Console.Write("\b-"); break;
                case 3: Console.Write("\b\\"); break;
            }

            ProgressCnt = (ProgressCnt + 1) % 4;
        }

    }
}
