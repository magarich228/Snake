using System;

namespace Snake
{
    class SpecialBonus : BaseObject
    {
        override public Type TypeOb
        {
            get => Type.SpecialBonus;
        }

        public SpecialBonus() : base("||")
        {

        }

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
