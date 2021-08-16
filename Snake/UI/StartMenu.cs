using System;

namespace Snake
{
    delegate void Start();
    struct StartMenu
    {
        private static ConsoleKeyInfo consoleKey;

        event Start start;
        event Start goDesign;
        private static StartMenu Menu;

        static StartMenu() 
        {
            Console.CursorVisible = false;

            Menu = new StartMenu();

            Menu.start = Menu.ShowMenu;
            Menu.start += Menu.Read;

            Menu.goDesign = Preservation.GetComponent().Launch;
        }

        public static void Launch() => Menu.start();

        private void ShowMenu()
        {
            Console.Clear();

#pragma warning disable CA1416 // Проверка совместимости платформы
            Console.WindowHeight = 24;
            Console.WindowWidth = 71;

            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            Console.SetCursorPosition(0, 2);
            Draw.DrawSnake();

            Console.SetCursorPosition(50, 3);
            Console.WriteLine($"1 Start");

            Console.SetCursorPosition(50, 4);
            Console.WriteLine($"2 MapDesigner");

            Console.SetCursorPosition(50, 5);
            Console.WriteLine($"3 exit");
        }

        private void Read()
        {
            consoleKey = Console.ReadKey(true);

            switch (consoleKey.Key)
            {
                case ConsoleKey.D1:
                    GameConfigure.Launch();
                    break;

                case ConsoleKey.D2:
                    Menu.goDesign();
                    break;

                case ConsoleKey.D3:

                    Console.Clear();
                    Console.WriteLine("Completion...");

                    Environment.Exit(1);
                    
                    return;

                default:
                    Menu.Read();
                    break;
            }
        }
    }
}
