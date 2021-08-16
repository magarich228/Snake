using System;

namespace Snake
{
    class Wall : BaseObject
    {
        override public Type TypeOb
        {
            get => Type.Wall;
        }

        public int X { get; private set; }
        
        public int Y { get; private set; }

        public Wall() : base("╬╬") { }

        public Wall(int Y, int X) : base("╬╬")
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
