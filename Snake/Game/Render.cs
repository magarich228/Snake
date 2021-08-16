using System;

namespace Snake
{
    class Render
    {
        private static Render render = new Render();

        private Render() { }

        public static Render GetComponent() => render;

        public void MatrixRender(Matrix matrix, int CursorLeft = 0, int CursorTop = 0, bool GameInterface = false)
        {
            Console.Clear();
            string OutPut = default;

            string[] Interface = GameInterface? new string[matrix.GetLong(Axis.y)] : null;
            if (GameInterface)
            {
                Interface.Initialize();

                Interface[0] = "Controls:";
                Interface[1] = "W, A, S, D";

                Interface[3] = "Escape - exit";
                Interface[4] = "R - restart";

                Interface[6] = $"F1 - pause ";

                Interface[8] = "Snakes on Map:";
                Interface[9] = $"{Engine.GetComponent().CountSnake()}";

                Interface[11] = $"Game pace {GameSettings.GetSettings().GameTempo}";
            }

            Console.SetCursorPosition(0, 0);

            int countLine = default;

            for (int CorY = 0; CorY < matrix.GetLong(Axis.y); CorY++)
            {
                for (int CorX = 0; CorX < matrix.GetLong(Axis.x); CorX++)
                {
                    if (matrix[CorY, CorX] != null)
                    {
                        switch (matrix[CorY, CorX].TypeOb)
                        {
                            case Type.Wall:
                                OutPut += (matrix[CorY, CorX] as Wall).symbol;
                                break;

                            case Type.Bonus:
                                OutPut += (matrix[CorY, CorX] as Bonus).symbol;
                                break;

                            case Type.Snake:
                                OutPut += (matrix[CorY, CorX] as Snake).symbol;
                                break;

                            case Type.SnakeTail:
                                OutPut += ((SnakeTail)matrix[CorY, CorX]).symbol;
                                break;

                            case Type.SpecialBonus:
                                OutPut += ((SpecialBonus)matrix[CorY, CorX]).symbol;
                                break;
                        }

                        OutPut += CorX == matrix.GetLong(Axis.x) - 1 ? GameInterface? $"|{Interface[countLine]}" : "|" : "";
                    }
                    else
                    {
                        OutPut += "  ";
                        OutPut += CorX == matrix.GetLong(Axis.x) - 1 ? GameInterface ? $"|{Interface[countLine]}" : "|" : "";
                    }
                }

                OutPut += "\n";

                countLine++;
            }

            //Console.SetCursorPosition(0, matrix.GetLong(Axis.y));

            for (int Count = 0; Count < matrix.GetLong(Axis.x); Count++)
            {
                OutPut += "==";
            }
            OutPut += "=";

            Console.SetCursorPosition(CursorLeft, CursorTop);
            Console.Write("\n" + OutPut);
        }
    }
}
