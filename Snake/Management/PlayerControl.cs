using System;

namespace Snake
{
    class PlayerControl : IController
    {
        private static PlayerControl playerControl;

        public Snake Snake { get; set; }

        public PlayerControl() { }

        public static PlayerControl GetComponent() => playerControl;

        public void Read()
        {
            ConsoleKeyInfo consoleKey = default;

            if (Console.KeyAvailable)
            {
                consoleKey = Console.ReadKey(true);

                while (Console.KeyAvailable)
                    Console.ReadKey(true);
            }

            switch (consoleKey.Key)
            {
                case ConsoleKey.W:
                    Snake.Direction = Snake.Direction == Direction.left || Snake.Direction == Direction.right ? Direction.up : Snake.Direction;
                    break;

                case ConsoleKey.S:
                    Snake.Direction = Snake.Direction == Direction.left || Snake.Direction == Direction.right ? Direction.down : Snake.Direction;
                    break;

                case ConsoleKey.A:
                    Snake.Direction = Snake.Direction == Direction.up || Snake.Direction == Direction.down ? Direction.left : Snake.Direction;
                    break;

                case ConsoleKey.D:
                    Snake.Direction = Snake.Direction == Direction.up || Snake.Direction == Direction.down ? Direction.right : Snake.Direction;
                    break;

                case ConsoleKey.UpArrow:
                    goto case ConsoleKey.W;

                case ConsoleKey.DownArrow:
                    goto case ConsoleKey.S;

                case ConsoleKey.LeftArrow:
                    goto case ConsoleKey.A;

                case ConsoleKey.RightArrow:
                    goto case ConsoleKey.D;

                case ConsoleKey.Escape:
                    Engine.GetComponent().EndCycle = true;
                    break;

                case ConsoleKey.F1:

                    while(!Console.KeyAvailable) { }
                    Read();

                    break;

                case ConsoleKey.R:
                    GameWindow.Run();
                    break;

                default:
                    break;
            }
        }
    }
}
