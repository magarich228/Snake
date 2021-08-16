using System;
using System.Threading;

namespace Snake
{
    struct GameConfigure
    {
        private static ConsoleKeyInfo consoleKeyInfo;

        event Start start;
        event Start Play;

        private static GameConfigure gameConfigure;

        static GameConfigure()
        {
            gameConfigure = new GameConfigure();

            gameConfigure.start = gameConfigure.ShowMenu;
            gameConfigure.start += gameConfigure.Read;

            gameConfigure.Play = GameWindow.Run;
            gameConfigure.Play += Engine.GetComponent().GameCycle;
        }

        public static void Launch() => gameConfigure.start();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
        private void ShowMenu()
        {
            Console.Clear();

            Console.WindowHeight = 24;
            Console.WindowWidth = 72;

            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            Console.SetCursorPosition(0, 2);
            Draw.DrawSnake();

            Console.SetCursorPosition(48, 3);
            Console.WriteLine($"1 Start");

            Console.SetCursorPosition(48, 4);
            Console.WriteLine($"2 Num of Bots: {GameSettings.GetSettings().NumOfBots}");

            Console.SetCursorPosition(48, 5);
            Console.WriteLine($"3 Is player exist? {GameSettings.GetSettings().PlayerExist}");

            Console.SetCursorPosition(48, 6);
            Console.WriteLine((GameSettings.GetSettings().Map != null) ? "4 Map: " + GameSettings.GetSettings().Map.Name : "4 Map: Empty");

            Console.SetCursorPosition(48, 7);
            Console.WriteLine($"5 Dynamic complexity?");
            Console.SetCursorPosition(50, 8);
            Console.WriteLine($"{GameSettings.GetSettings().DynamicComplexity}");

            Console.SetCursorPosition(48, 9);
            Console.WriteLine($"6 Game tempo: {GameSettings.GetSettings().GameTempo}");

            Console.SetCursorPosition(48, 10);
            Console.WriteLine($"7 Back");
        }
        private void Read()
        {
            consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.D1:

                    if (GameSettings.GetSettings().Map == null)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Map is not selected!");

                        Thread.Sleep(1000);
                        Console.Clear();

                        ShowMenu();
                        Read();

                        return;
                    } 
                    else if (GameSettings.GetSettings().NumOfBots == 0 && GameSettings.GetSettings().PlayerExist == false)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("there will be" +
                            "\nno snakes on the map!");

                        Thread.Sleep(1000);
                        Console.Clear();

                        ShowMenu();
                        Read();

                        return;
                    }

                    gameConfigure.Play();
                    StartMenu.Launch();

                    break;

                case ConsoleKey.D2:

                    Console.CursorVisible = true;
                    Console.SetCursorPosition(63, 4);
                    Console.Write("       ");

                    Console.SetCursorPosition(63, 4);
                    try
                    {
                        int num = int.Parse(Console.ReadLine());

                        GameSettings.GetSettings().NumOfBots = num >= 0 && num <= 10? num: GameSettings.GetSettings().NumOfBots;
                    }
                    catch (FormatException)
                    {
                        Console.CursorVisible = false;
                        Launch();

                        return;
                    }

                    Console.CursorVisible = false;
                    Launch();

                    break;

                case ConsoleKey.D3:

                    GameSettings.GetSettings().PlayerExist = !GameSettings.GetSettings().PlayerExist;
                    Launch();

                    break;

                case ConsoleKey.D4:

                    Console.SetCursorPosition(0, 0);

                    int count = 1;
                    if (GameComponents.GetLastIndex() != 0)
                    {
                        for (int index = 0; index < GameComponents.GetLastIndex(); index++)
                        {
                            Console.WriteLine(count + "." + GameComponents.Maps[index].Name);

                            count++;
                        }

                        Console.WriteLine(GameComponents.Maps[9] != null ? GameComponents.Maps[9].Name : "");
                    }
                    else
                    {
                        Console.WriteLine("Empty");
                        Thread.Sleep(2000);

                        Launch();
                    }

                    Console.WriteLine("Escape - cancel");

                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                        Launch();


                    int indexNum = default;
                    try
                    {
                        indexNum = int.Parse(key.KeyChar.ToString());
                    }

                    catch (FormatException) { goto case ConsoleKey.D4; }
                    catch (OverflowException) { goto case ConsoleKey.D4; }

                    GameSettings.GetSettings().Map = indexNum > 0 & indexNum <= GameComponents.GetLastIndex() ? GameComponents.Maps[--indexNum] : null;

                    Launch();

                    break;

                case ConsoleKey.D5:

                    GameSettings.GetSettings().DynamicComplexity = !GameSettings.GetSettings().DynamicComplexity;
                    Launch();

                    break;

                case ConsoleKey.D6:

                    Console.CursorVisible = true;
                    Console.SetCursorPosition(62, 9);
                    Console.Write("       ");

                    Console.SetCursorPosition(62, 9);
                    try
                    {
                        int temp = int.Parse(Console.ReadLine());

                        GameSettings.GetSettings().GameTempo = temp >= 50 ? temp <= 2000 ? temp : GameSettings.GetSettings().GameTempo : GameSettings.GetSettings().GameTempo;
                    }
                    catch (FormatException)
                    {
                        Console.CursorVisible = false;
                        Launch();

                        return;
                    }

                    Console.CursorVisible = false;
                    Launch();

                    break;

                case ConsoleKey.D7:

                    GameSettings.RefreshSettings();
                    StartMenu.Launch();

                    break;

                default:
                    gameConfigure.Read();
                    break;
            }
        }
    }
}
