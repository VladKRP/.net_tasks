using System;
using System.Linq;

namespace ExceptionHandlingModule
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Type string:");
                while (true)
                {
                    var userInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(userInput))
                    {
                        System.Console.WriteLine(userInput[0]);
                    }
                    else
                    {
                        System.Console.WriteLine("Your input should not be empty.");
                    }
                }
        }

    }
}
