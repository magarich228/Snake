using System;

namespace Snake
{
    class Coordinate
    {
        public int X;
        public int Y;

        public Coordinate(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    class LinkedGroup
    {
        public Coordinate Base;

        public Coordinate Set;

        public Coordinate[] Followers;

        public LinkedGroup(int x, int y)
        {
            Set = new Coordinate(x, y);

            Base = null;
            Followers = new Coordinate[4];
        }
    }

    class StructCoordinate
    {
        LinkedGroup[][] List;

        private static Matrix matrix = GameSettings.GetSettings().Map.matrix;

        private LiMap map = new LiMap(matrix.GetLong(Axis.y), matrix.GetLong(Axis.x));

        public StructCoordinate(int x, int y)
        {
            List = new LinkedGroup[1][];
            List[0] = new LinkedGroup[1];
            List[0][0] = new LinkedGroup(x, y);
        }

        public void Add()
        {
            Array.Resize(ref List, (int)List.GetLongLength(0) + 1);
            List[(int)List.GetLongLength(0) - 1] = new LinkedGroup[0];

            for (int index = 0; index < List[(int)List.GetLongLength(0) - 2].Length; index++)
            {
                LinkedGroup tempLG = List[(int)List.GetLongLength(0) - 2][index];

                if (matrix[tempLG.Set.Y - 1, tempLG.Set.X] == null ||
                    matrix[tempLG.Set.Y - 1, tempLG.Set.X] is Bonus ||
                    matrix[tempLG.Set.Y - 1, tempLG.Set.X] is SpecialBonus)
                {
                    tempLG.Followers[0] = !map[tempLG.Set.Y - 1, tempLG.Set.X] ?
                        new Coordinate(tempLG.Set.X, tempLG.Set.Y - 1) : null;

                    if (tempLG.Followers[0] != null)
                    {
                        Array.Resize(ref List[(int)List.GetLongLength(0) - 1], List[(int)List.GetLongLength(0) - 1].Length + 1);

                        List[(int)List.GetLongLength(0) - 1][List[(int)List.GetLongLength(0) - 1].Length - 1] =
                            new LinkedGroup(tempLG.Followers[0].X, tempLG.Followers[0].Y) { Base = tempLG.Set };

                        map[tempLG.Followers[0].Y, tempLG.Followers[0].X] = true;
                    }
                }

                if (matrix[tempLG.Set.Y, tempLG.Set.X + 1] == null ||
                    matrix[tempLG.Set.Y, tempLG.Set.X + 1] is Bonus ||
                    matrix[tempLG.Set.Y, tempLG.Set.X + 1] is SpecialBonus)
                {
                    tempLG.Followers[1] = !map[tempLG.Set.Y, tempLG.Set.X + 1] ?
                        new Coordinate(tempLG.Set.X + 1, tempLG.Set.Y) : null;

                    if (tempLG.Followers[1] != null)
                    {
                        Array.Resize(ref List[(int)List.GetLongLength(0) - 1], List[(int)List.GetLongLength(0) - 1].Length + 1);

                        List[(int)List.GetLongLength(0) - 1][List[(int)List.GetLongLength(0) - 1].Length - 1] =
                            new LinkedGroup(tempLG.Followers[1].X, tempLG.Followers[1].Y) { Base = tempLG.Set };

                        map[tempLG.Followers[1].Y, tempLG.Followers[1].X] = true;
                    }
                }

                if (matrix[tempLG.Set.Y + 1, tempLG.Set.X] == null ||
                    matrix[tempLG.Set.Y + 1, tempLG.Set.X] is Bonus ||
                    matrix[tempLG.Set.Y + 1, tempLG.Set.X] is SpecialBonus)
                {
                    tempLG.Followers[2] = !map[tempLG.Set.Y + 1, tempLG.Set.X] ?
                        new Coordinate(tempLG.Set.X, tempLG.Set.Y + 1) : null;

                    if (tempLG.Followers[2] != null)
                    {
                        Array.Resize(ref List[(int)List.GetLongLength(0) - 1], List[(int)List.GetLongLength(0) - 1].Length + 1);

                        List[(int)List.GetLongLength(0) - 1][List[(int)List.GetLongLength(0) - 1].Length - 1] =
                            new LinkedGroup(tempLG.Followers[2].X, tempLG.Followers[2].Y) { Base = tempLG.Set };

                        map[tempLG.Followers[2].Y, tempLG.Followers[2].X] = true;
                    }
                }

                if (matrix[tempLG.Set.Y, tempLG.Set.X - 1] == null ||
                    matrix[tempLG.Set.Y, tempLG.Set.X - 1] is Bonus ||
                    matrix[tempLG.Set.Y, tempLG.Set.X - 1] is SpecialBonus)
                {
                    tempLG.Followers[3] = !map[tempLG.Set.Y, tempLG.Set.X - 1] ?
                        new Coordinate(tempLG.Set.X - 1, tempLG.Set.Y) : null;

                    if (tempLG.Followers[3] != null)
                    {
                        Array.Resize(ref List[(int)List.GetLongLength(0) - 1], List[(int)List.GetLongLength(0) - 1].Length + 1);

                        List[(int)List.GetLongLength(0) - 1][List[(int)List.GetLongLength(0) - 1].Length - 1] =
                            new LinkedGroup(tempLG.Followers[3].X, tempLG.Followers[3].Y) { Base = tempLG.Set };

                        map[tempLG.Followers[3].Y, tempLG.Followers[3].X] = true;
                    }
                }

            }
        }

        public LinkedGroup Check(int y, int x)
        {
            if (List[List.Length - 1].Length == 0)
                return List[List.Length - 2][new Random().Next(0, List[List.Length - 2].Length)];
            
            foreach (LinkedGroup dot in List[List.Length - 1])
            {
                if (dot.Set.X == x && dot.Set.Y == y)
                    return dot;
            }

            return null;
        }

        public LinkedGroup GetLinks(Coordinate coordinate)
        {
            foreach (LinkedGroup[] list in List)
                foreach (LinkedGroup dot in list)
                    if (dot.Set.X == coordinate.X && dot.Set.Y == coordinate.Y)
                        return dot;

            return null;
        }
    }

    class LiMap
    {
        bool[,] Map;

        public LiMap(int ylength, int xlength) => Map = new bool[ylength, xlength];

        public bool this[int y, int x]
        {
            get
            {
                int Y = y < 0 ? y + (int)Map.GetLongLength(0) : y >= (int)Map.GetLongLength(0) ?
                    y - (int)Map.GetLongLength(0) : y,
                X = x < 0 ? x + (int)Map.GetLongLength(1) : x >= (int)Map.GetLongLength(1) ?
                    x - (int)Map.GetLongLength(1) : x;

                return Map[Y, X];
            }
            set
            {
                int Y = y < 0 ? y + (int)Map.GetLongLength(0) : y >= (int)Map.GetLongLength(0) ?
                    y - (int)Map.GetLongLength(0) : y,
                X = x < 0 ? x + (int)Map.GetLongLength(1) : x >= (int)Map.GetLongLength(1) ?
                    x - (int)Map.GetLongLength(1) : x;

                Map[Y, X] = value;
            }
        }
    }
}