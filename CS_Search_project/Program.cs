using System;

namespace _04_UIL
{
    class Program
    {
        static void Main(string[] args)
        {
            while (UI_functions.Startup() != 4)
            {
                Console.WriteLine("Press any key to return to main menu");
                Console.ReadLine();
            }
        }
    }
}
