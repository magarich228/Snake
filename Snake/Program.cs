using System;
using System.Text;

namespace Snake
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Default;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            StartMenu.Launch();
        }
    }
}