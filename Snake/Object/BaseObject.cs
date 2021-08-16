using System;

namespace Snake
{
    abstract class BaseObject
    {
        public string symbol { get; protected set; }
        protected int x;
        protected int y;

        public virtual Type TypeOb { get; }

        protected BaseObject(string ch) => symbol = ch;

        public void CheckCoordinate()
        {
            x = x < 0 ? x + GameSettings.GetSettings().Map.matrix.GetLong(Axis.x) : x;
            x = x > GameSettings.GetSettings().Map.matrix.GetLong(Axis.x) - 1? 
                x - GameSettings.GetSettings().Map.matrix.GetLong(Axis.x) : x;

            y = y < 0 ? y + GameSettings.GetSettings().Map.matrix.GetLong(Axis.y) : y;
            y = y > GameSettings.GetSettings().Map.matrix.GetLong(Axis.y) - 1 ?
                y - GameSettings.GetSettings().Map.matrix.GetLong(Axis.y) : y;
        }
    }

    enum Type
    {
        Snake,
        SnakeTail,
        Wall,
        Bonus,
        SpecialBonus
    }
}
