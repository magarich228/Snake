using System;
using System.Threading;

namespace Snake
{
    class GameWindow
    {
        public static Generate generate;

        static GameWindow() => generate = new Generate();

        public static void Run()
        {
            Console.CursorVisible = false;

#pragma warning disable CA1416 // Проверка совместимости платформы
            Console.WindowHeight = GameSettings.GetSettings().Map.matrix.GetLong(Axis.y) + 1;
            Console.WindowWidth = GameSettings.GetSettings().Map.matrix.GetLong(Axis.x) * 2 + 16;

            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            GameSettings.GetSettings().Map.matrix.DownloadFromFile(ToFile.ToSave);

            if (!generate.GenerateSnake())
            {
                Console.Clear();
                Console.WriteLine("not enough space to place snakes on the map!");

                Thread.Sleep(2500);

                GameConfigure.Launch();
            }
            
            Engine.GetComponent().InitializeComponent();

            Render.GetComponent().MatrixRender(GameSettings.GetSettings().Map.matrix, 0, 0, true);

            Console.SetCursorPosition(GameSettings.GetSettings().Map.matrix.GetLong(Axis.x) * 2 + 1, GameSettings.GetSettings().Map.matrix.GetLong(Axis.y) - 1);
            Console.Write("Press any key");

            Console.SetCursorPosition(GameSettings.GetSettings().Map.matrix.GetLong(Axis.x) * 2 + 1, GameSettings.GetSettings().Map.matrix.GetLong(Axis.y));
            Console.Write("to Start.");

            Console.ReadKey(true);
        }
    }
}
