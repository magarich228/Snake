using System;
using System.Threading;

namespace Snake
{
    class Engine
    {
        private static Engine GameEngine;

        public static GameSettings ThisGameSettings { get; private set; }

        public static Generate GameGenerate { get; private set; }


        public static Snake[] AllSnake;

        public bool EndCycle;

        private int GameCycleItaretion;



        public event Move MoveAllSnake;

        event Move GenerateBonuses;

        static Engine() => GameEngine = new Engine();

        public void InitializeComponent()
        {
            GameGenerate = new Generate();

            ThisGameSettings = GameSettings.GetSettings();

            AllSnake = new Snake[GameComponents.Bots.Length + (ThisGameSettings.PlayerExist ? 1 : 0)]; //last index - player

            for (int index = 0; index < AllSnake.Length; index++)
            {
                if (index == GameComponents.Bots.Length)
                {
                    AllSnake[index] = GameComponents.Player;
                    break;
                }

                AllSnake[index] = GameComponents.Bots[index];
            }

            GameEngine.MoveAllSnake = ThisGameSettings.PlayerExist ? GameComponents.Player.Move : GameEngine.MoveAllSnake;

            foreach (Snake snake in GameComponents.Bots)
                GameEngine.MoveAllSnake += snake.Move;

            GameEngine.GenerateBonuses = GameGenerate.GenerateBonuses;

            GameEngine.EndCycle = false;
            GameEngine.GameCycleItaretion = default;
        }

        private Engine() { }

        public static Engine GetComponent() => GameEngine;

        private void Update()
        {
            Collaiders.RefreshCollaiders();
            GameGenerate.RefreshBonuses();

            if (GameEngine.MoveAllSnake != null)
                GameEngine.MoveAllSnake();

            CheckCoordinate();

            if (GameCycleItaretion % 30 == 0 || GameGenerate.Bonuses.Length == 0)
                GameEngine.GenerateBonuses();

            GameEngine.SeekCollision();

            RemoveNullInAllSnake();
            RemoveNullInSnake(ref GameComponents.Bots);
        }

        public void GameCycle()
        {
            GameEngine.GenerateBonuses();
            Render.GetComponent().MatrixRender(ThisGameSettings.Map.matrix, 0, 0, true);

            while (!EndCycle)
            {
                if (AllSnake.Length == 0)
                    GameWindow.Run();

                if (GameComponents.Player != null)
                    GameComponents.Player?.Control.Read();
                else Read();

                for (int index = 0; index < AllSnake.Length - (GameComponents.Player != null ? 1: 0); index++)
                    AllSnake[index].Control.Read();

                GameEngine.Update();

                DownloadSnakesOnMatrix();
                Render.GetComponent().MatrixRender(ThisGameSettings.Map.matrix, 0, 0, true);

                Thread.Sleep(ThisGameSettings.GameTempo);

                GameEngine.GameCycleItaretion++;
            }
        }

        private void SeekCollision()
        {
            for (int index = 0; index < AllSnake.Length; index++)
            {
                if (AllSnake[index] != null)
                    for (int wallsIndex = 0; wallsIndex < Collaiders.Walls.Length; wallsIndex++)
                    {
                        if (AllSnake[index] != null && AllSnake[index].X == Collaiders.Walls[wallsIndex].X
                            && AllSnake[index].Y == Collaiders.Walls[wallsIndex].Y)

                            AllSnake[index].Death();
                    }

                if (AllSnake[index] != null)
                    for (int snakesTailsIndex = 0; snakesTailsIndex < Collaiders.SnakesTails.Length; snakesTailsIndex++)
                    {
                        if (AllSnake[index] != null && AllSnake[index].X == Collaiders.SnakesTails[snakesTailsIndex].X
                            && AllSnake[index].Y == Collaiders.SnakesTails[snakesTailsIndex].Y)

                            AllSnake[index].Death();
                    }

                if (AllSnake[index] != null)
                    for (int snakesHeadIndex = 0; snakesHeadIndex < AllSnake.Length; snakesHeadIndex++)
                    {
                        if (AllSnake[index] != null && AllSnake[snakesHeadIndex] != null && AllSnake[index].GetHashCode() != AllSnake[snakesHeadIndex].GetHashCode() &&
                            AllSnake[index].X == AllSnake[snakesHeadIndex].X && AllSnake[index].Y == AllSnake[snakesHeadIndex].Y)
                        {
                            AllSnake[index].Death();

                            AllSnake[snakesHeadIndex].Death();
                        }
                    }

                if (AllSnake[index] != null)
                    for (int allBonusesIndex = 0; GameGenerate.Bonuses != null && allBonusesIndex < GameGenerate.Bonuses.Length; allBonusesIndex++)
                    {
                        if (GameGenerate.Bonuses[allBonusesIndex] == null)
                            continue;

                        switch (GameGenerate.Bonuses[allBonusesIndex].TypeOb)
                        {
                            case Type.Bonus:

                                if (AllSnake[index].Y == ((Bonus)GameGenerate.Bonuses[allBonusesIndex]).Y &&
                                    AllSnake[index].X == ((Bonus)GameGenerate.Bonuses[allBonusesIndex]).X)
                                {
                                    AllSnake[index].Grow(numOfElement: 1);

                                    GameGenerate.Bonuses[allBonusesIndex] = null;
                                }

                                break;

                            case Type.SpecialBonus:

                                if (AllSnake[index].Y == ((SpecialBonus)GameGenerate.Bonuses[allBonusesIndex]).Y &&
                                    AllSnake[index].X == ((SpecialBonus)GameGenerate.Bonuses[allBonusesIndex]).X)
                                {
                                    AllSnake[index].Grow(numOfElement: 3);

                                    GameGenerate.Bonuses[allBonusesIndex] = null;
                                }

                                break;
                        }
                    }
            }
        }

        public int CountSnake()
        {
            int count = 0;

            for (int index = 0; index < AllSnake.Length; index++)
                count += AllSnake[index] != null ? 1 : 0;

            return count;
        }
        public int CountSnake(BaseObject[] mass)
        {
            int count = 0;

            for (int index = 0; index < mass.Length; index++)
                count += mass[index] != null ? 1 : 0;

            return count;
        }

        public static void DownloadSnakesOnMatrix()
        {
            Matrix matrix = ThisGameSettings.Map.matrix;

            matrix.ClearMatrixOfSnakes();

            for (int count = 0; count < AllSnake.Length; count++)
            {
                if (AllSnake[count] != null)
                {
                    matrix[AllSnake[count].Y, AllSnake[count].X] = AllSnake[count];

                    for (int TailIndex = 0; TailIndex < AllSnake[count].snakeTails.Length; TailIndex++)
                    {
                        matrix[AllSnake[count].snakeTails[TailIndex].Y, AllSnake[count].snakeTails[TailIndex].X] = AllSnake[count].snakeTails[TailIndex];
                    }
                }
            }
        }

        private static void CheckCoordinate()
        {
            for (int count = 0; count < AllSnake.Length; count++)
            {
                if (AllSnake[count] != null)
                {
                    AllSnake[count].CheckCoordinate();

                    for (int TailIndex = 0; TailIndex < AllSnake[count].snakeTails.Length; TailIndex++)
                    {
                        AllSnake[count].snakeTails[TailIndex].CheckCoordinate();
                    }
                }
            }
        }

        public static void RemoveMoveEvent(Move move) => GameEngine.MoveAllSnake -= move;

        private static void RemoveNullInAllSnake()
        {
            Snake[] Buffer = new Snake[GameEngine.CountSnake()];

            for (int count = 0; count < AllSnake.Length; count++)
            {
                if (Buffer.Length != 0 && GameEngine.CountSnake(Buffer) != Buffer.Length)
                    Buffer[GameEngine.CountSnake(Buffer)] = AllSnake[count] != null ? AllSnake[count] : null;
            }

            Array.Resize(ref AllSnake, Buffer.Length);

            for (int count = 0; count < AllSnake.Length; count++)
            {
                AllSnake[count] = Buffer[count];
            }
        }

        private static void RemoveNullInSnake(ref Snake[] mass)
        {
            Snake[] Buffer = new Snake[GameEngine.CountSnake(mass)];

            for (int count = 0; count < mass.Length; count++)
            {
                if (Buffer.Length != 0 && GameEngine.CountSnake(Buffer) != Buffer.Length)
                    Buffer[GameEngine.CountSnake(Buffer)] = mass[count] != null ? mass[count] : null;
            }

            Array.Resize(ref mass, Buffer.Length);

            for (int count = 0; count < mass.Length; count++)
            {
                mass[count] = Buffer[count];
            }
        }

        private static void Read()
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
                case ConsoleKey.Escape:
                    GetComponent().EndCycle = true;
                    break;

                case ConsoleKey.F1:
                    while (!Console.KeyAvailable) { }
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