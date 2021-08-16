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

            if (GameSettings.GetSettings().Map == null)
            {
                Console.Clear();
                Console.WriteLine("Map is not selected.");

                Thread.Sleep(2000);

                return;
            }

#pragma warning disable CA1416 // Проверка совместимости платформы
            Console.WindowHeight = GameSettings.GetSettings().Map.matrix.GetLong(Axis.y) + 1;
            Console.WindowWidth = GameSettings.GetSettings().Map.matrix.GetLong(Axis.x) * 2 + 16;

            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            GameSettings.GetSettings().Map.matrix.DownloadFromFile(ToFile.ToSave);

            generate.GenerateSnake();
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
