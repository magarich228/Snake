using System;

namespace Snake
{
    delegate void Run(bool New);
    delegate void RunOld(string name);
    class Preservation
    {
        private static Preservation preservation;
        event Run goDesignNew;
        event RunOld goDesignOld;


        ConsoleKeyInfo consoleKeyInfo;

        private Preservation() { }
        public void Launch()
        {
            preservation.ShowMenu();
            preservation.Read();
        }
        static Preservation()
        {
            preservation = new Preservation();

            preservation.goDesignNew = MapDesigner.GetComponent().Launch;
            preservation.goDesignOld = MapDesigner.GetComponent().Launch;
        }

        public static Preservation GetComponent() => preservation;

        private void ShowMenu()
        {
            Console.Clear();

#pragma warning disable CA1416 // Проверка совместимости платформы
            Console.WindowHeight = 20;
            Console.WindowWidth = 35;

            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            Console.WriteLine($"1 Create new Map");
            Console.WriteLine($"2 Back\n");

            if (GameComponents.GetLastIndex() != 0)
            {
                for (int index = 0; index < GameComponents.GetLastIndex(); index++)
                {
                    Console.WriteLine(GameComponents.Maps[index].Name);
                }

                Console.WriteLine(GameComponents.Maps[9] != null ? GameComponents.Maps[9].Name: "");
            }
            else
                Console.WriteLine("Empty");
        }

        private void Read(byte topPosition = 3, int countIndexForMapname = 0)
        {
            topPosition = topPosition < 3 ? (byte)3 : topPosition >= ((byte)GameComponents.GetLastIndex() + 2) ?
                (byte)(GameComponents.GetLastIndex() + 2) : topPosition;

            int countIndexForMapName = countIndexForMapname < 0? 0 : 
                countIndexForMapname >= GameComponents.GetLastIndex() - 1? GameComponents.GetLastIndex() - 1: countIndexForMapname;

            Console.CursorVisible = GameComponents.GetLastIndex() > 0 ? true: false;
            Console.SetCursorPosition(0, topPosition);
            
            consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.D1:
                    
                    if (GameComponents.GetLastIndex() < GameComponents.Maps.Length)
                        preservation.goDesignNew(true);
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("The maximum number of maps is 10!");

                        Launch();
                    }

                    break;

                case ConsoleKey.D2:

                    Console.CursorVisible = false;
                    StartMenu.Launch();

                    break;

                case ConsoleKey.S:
                    countIndexForMapName++;

                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
                    Read(Convert.ToByte(Console.CursorTop), countIndexForMapName);

                    break;

                case ConsoleKey.W:
                    countIndexForMapName--;

                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    Read(Convert.ToByte(Console.CursorTop), countIndexForMapName);

                    break;

                case ConsoleKey.Enter:

                    if (GameComponents.GetLastIndex() > 0)
                        preservation.goDesignOld(GameComponents.Maps[countIndexForMapName].Name);

                    Read(topPosition, countIndexForMapName);

                    break;

                case ConsoleKey.Delete:

                    if (GameComponents.Maps[0] != null)
                    {
                        GameComponents.RemoveMap(countIndexForMapName);
                        ShowMenu();
                    }

                    Read(topPosition, countIndexForMapName);

                    break;

                default:

                    ShowMenu();
                    Read(topPosition, countIndexForMapName);

                    break;
            }
        }
    }
}
