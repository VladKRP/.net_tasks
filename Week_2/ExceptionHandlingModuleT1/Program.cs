using System;

namespace ExceptionHandlingModuleT1
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true){
                System.Console.WriteLine("Type something:");
                var userInput = Console.ReadLine();
                if(!string.IsNullOrWhiteSpace(userInput))
                    System.Console.WriteLine(userInput[0]);
                else 
                    System.Console.WriteLine("User input must contain at least one character.\n" +
                                             "We suppose you pass empty string.");
            }
        }
    }
}
