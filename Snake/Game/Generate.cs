using System;

namespace Snake
{
    class Generate
    {
        private static Matrix matrix;

        private static Random rnd = new Random();

        public BaseObject[] Bonuses = new BaseObject[0];

        static Generate() => matrix = GameSettings.GetSettings().Map.matrix;
        
        public void GenerateSnake() // В будущем допилить и сделать более безопасный спавн за счет метода нахождение пути и его длины
        {
            GameComponents.Bots = new Snake[GameSettings.GetSettings().NumOfBots];
            GameComponents.Player = GameSettings.GetSettings().PlayerExist ? new Snake("@@", new PlayerControl()) : null;

            for (int count = 0; count < GameComponents.Bots.Length; count++)
                GameComponents.Bots[count] = new Snake("##", new AI(GameComponents.Bots[count]));

            int countIteration = 0, maxIteration = matrix.GetLong(Axis.y) * matrix.GetLong(Axis.x) * 4 * 4;

            for (int countSnake = 0; countSnake < GameComponents.Bots.Length + (GameComponents.Player != null ? 1 : 0); countSnake++)
            {
                if (countIteration == maxIteration)
                    throw new Exception("Not enough space on the map to accommodate so many snakes!");

                countIteration++;

                int x = rnd.Next(0, matrix.GetLong(Axis.x)),
                    y = rnd.Next(0, matrix.GetLong(Axis.y));

                Direction dir = (Direction)rnd.Next(0, 4);

                if (matrix[y, x] != null)
                {
                    countSnake--;
                    continue;
                }

                switch (dir)
                {
                    case Direction.up:

                        if (y - 1 >= 0 && y + 3 < matrix.GetLong(Axis.y) && matrix[y - 1, x] == null
                            && matrix[y + 1, x] == null && matrix[y + 2, x] == null && matrix[y + 3, x] == null)
                        {
                            matrix[y, x] = countSnake < GameComponents.Bots.Length ? GameComponents.Bots[countSnake] :
                                GameComponents.Player != null ? GameComponents.Player : null;

                            ((Snake)matrix[y, x]).Direction = dir;
                            ((Snake)matrix[y, x]).X = x;
                            ((Snake)matrix[y, x]).Y = y;

                            matrix[y + 1, x] = ((Snake)matrix[y, x]).snakeTails[0];
                            matrix[y + 2, x] = ((Snake)matrix[y, x]).snakeTails[1];
                            matrix[y + 3, x] = ((Snake)matrix[y, x]).snakeTails[2];

                            ((SnakeTail)matrix[y + 1, x]).X = x;
                            ((SnakeTail)matrix[y + 1, x]).Y = y + 1;

                            ((SnakeTail)matrix[y + 2, x]).X = x;
                            ((SnakeTail)matrix[y + 2, x]).Y = y + 2;

                            ((SnakeTail)matrix[y + 3, x]).X = x;
                            ((SnakeTail)matrix[y + 3, x]).Y = y + 3;
                        }
                        else 
                        {
                            countSnake--;
                            continue;
                        }

                        break;

                    case Direction.left:

                        if (x - 1 >= 0 && x + 3 < matrix.GetLong(Axis.x) && matrix[y, x - 1] == null
                            && matrix [y, x + 1] == null && matrix[y, x + 2] == null && matrix[y, x + 3] == null)
                        {
                            matrix[y, x] = countSnake < GameComponents.Bots.Length ? GameComponents.Bots[countSnake] :
                                GameComponents.Player != null ? GameComponents.Player : null;

                            ((Snake)matrix[y, x]).Direction = dir;
                            ((Snake)matrix[y, x]).X = x;
                            ((Snake)matrix[y, x]).Y = y;

                            matrix[y, x + 1] = ((Snake)matrix[y, x]).snakeTails[0];
                            matrix[y, x + 2] = ((Snake)matrix[y, x]).snakeTails[1];
                            matrix[y, x + 3] = ((Snake)matrix[y, x]).snakeTails[2];

                            ((SnakeTail)matrix[y, x + 1]).X = x + 1;
                            ((SnakeTail)matrix[y, x + 1]).Y = y;

                            ((SnakeTail)matrix[y, x + 2]).X = x + 2;
                            ((SnakeTail)matrix[y, x + 2]).Y = y;

                            ((SnakeTail)matrix[y, x + 3]).X = x + 3;
                            ((SnakeTail)matrix[y, x + 3]).Y = y;
                        }
                        else
                        {
                            countSnake--;
                            continue;
                        }

                        break;

                    case Direction.down:

                        if (y + 1 < matrix.GetLong(Axis.y) && y - 3 >= 0 && matrix[y + 1, x] == null
                            && matrix[y - 1, x] == null && matrix[y - 2, x] == null && matrix[y - 3, x] == null)
                        {
                            matrix[y, x] = countSnake < GameComponents.Bots.Length ? GameComponents.Bots[countSnake] :
                                GameComponents.Player != null ? GameComponents.Player : null;

                            ((Snake)matrix[y, x]).Direction = dir;
                            ((Snake)matrix[y, x]).X = x;
                            ((Snake)matrix[y, x]).Y = y;

                            matrix[y - 1, x] = ((Snake)matrix[y, x]).snakeTails[0];
                            matrix[y - 2, x] = ((Snake)matrix[y, x]).snakeTails[1];
                            matrix[y - 3, x] = ((Snake)matrix[y, x]).snakeTails[2];

                            ((SnakeTail)matrix[y - 1, x]).X = x;
                            ((SnakeTail)matrix[y - 1, x]).Y = y - 1;

                            ((SnakeTail)matrix[y - 2, x]).X = x;
                            ((SnakeTail)matrix[y - 2, x]).Y = y - 2;

                            ((SnakeTail)matrix[y - 3, x]).X = x;
                            ((SnakeTail)matrix[y - 3, x]).Y = y - 3;
                        }
                        else
                        {
                            countSnake--;
                            continue;
                        }

                        break;

                    case Direction.right:

                        if (x - 3 >= 0 && x + 1 < matrix.GetLong(Axis.x) && matrix[y, x + 1] == null
                            && matrix[y, x - 1] == null && matrix[y, x - 2] == null && matrix[y, x - 3] == null)
                        {
                            matrix[y, x] = countSnake < GameComponents.Bots.Length ? GameComponents.Bots[countSnake] :
                                GameComponents.Player != null ? GameComponents.Player : null;

                            ((Snake)matrix[y, x]).Direction = dir;
                            ((Snake)matrix[y, x]).X = x;
                            ((Snake)matrix[y, x]).Y = y;

                            matrix[y, x - 1] = ((Snake)matrix[y, x]).snakeTails[0];
                            matrix[y, x - 2] = ((Snake)matrix[y, x]).snakeTails[1];
                            matrix[y, x - 3] = ((Snake)matrix[y, x]).snakeTails[2];

                            ((SnakeTail)matrix[y, x - 1]).X = x - 1;
                            ((SnakeTail)matrix[y, x - 1]).Y = y;

                            ((SnakeTail)matrix[y, x - 2]).X = x - 2;
                            ((SnakeTail)matrix[y, x - 2]).Y = y;

                            ((SnakeTail)matrix[y, x - 3]).X = x - 3;
                            ((SnakeTail)matrix[y, x - 3]).Y = y;
                        }
                        else
                        {
                            countSnake--;
                            continue;
                        }

                        break;
                }
            }
        }

        public void GenerateBonuses() 
        {
            int count = Engine.GetComponent().CountSnake() / 2 + Engine.GetComponent().CountSnake() % 2;

            while (count > 0)
            {
                int x = rnd.Next(0, matrix.GetLong(Axis.x)), y = rnd.Next(0, matrix.GetLong(Axis.y));

                BaseObject bonus = (matrix[y, x] == null) ? (rnd.Next(0, 101) > 15) ? new Bonus() : new SpecialBonus(): null;

                if (bonus != null)
                {
                    matrix[y, x] = bonus;
                    
                    if (bonus is Bonus)
                    {
                        (bonus as Bonus).X = x;
                        (bonus as Bonus).Y = y;
                    }
                    else
                    {
                        (bonus as SpecialBonus).X = x;
                        (bonus as SpecialBonus).Y = y;
                    }

                    Array.Resize(ref this.Bonuses, Bonuses.Length + 1);
                    this.Bonuses[Bonuses.Length - 1] = bonus;
                }
                else
                    continue;

                count--;
            }
        }

        public void RefreshBonuses()
        {
            BaseObject[] Buffer = new BaseObject[0];

            for (int index = 0; index < Bonuses.Length; index++)
            {
                if (Bonuses[index] == null)
                    continue;

                Array.Resize(ref Buffer, Buffer.Length + 1);
                Buffer[Buffer.Length - 1] = Bonuses[index];
            }

            Bonuses = Buffer;
        }
    }
}
