using System;

namespace Snake
{
    static class Collaiders
    {
        private static GameSettings ThisGameSettings;

        private static Matrix GameMatrix;

        public static Wall[] Walls;

        public static SnakeTail[] SnakesTails;

        static Collaiders()
        {
            ThisGameSettings = GameSettings.GetSettings();
            GameMatrix = GameSettings.GetSettings().Map.matrix;
        }

        public static void RefreshCollaiders()
        {
            Walls = new Wall[0];
            SnakesTails = new SnakeTail[0];

            for (int Y = 0; Y < ThisGameSettings.Map.matrix.GetLong(Axis.y); Y++)
            {
                for (int X = 0; X < ThisGameSettings.Map.matrix.GetLong(Axis.x); X++)
                {
                    if (GameMatrix[Y, X] != null)
                        switch (GameMatrix[Y, X].TypeOb)
                        {
                            case Type.Wall:

                                Array.Resize(ref Walls, Walls.Length + 1);
                                Walls[Walls.Length - 1] = new Wall(Y, X);

                                break;

                            case Type.SnakeTail:

                                Array.Resize(ref SnakesTails, SnakesTails.Length + 1);
                                SnakesTails[SnakesTails.Length - 1] = (SnakeTail)GameMatrix[Y, X];

                                break;
                        }
                }
            }
        }
    }
}
