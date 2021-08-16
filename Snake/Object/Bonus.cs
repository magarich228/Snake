using System;

namespace Snake
{
    class Bonus : BaseObject
    {
        //├┤
        override public Type TypeOb 
        { 
            get => Type.Bonus;
        }

        public Bonus() : base("├┤") { }

        public int X
        {
            get { return x; }

            set { x = value; }
        }

        public int Y
        {
            get { return y; }

            set { y = value; }
        }
    }
}
