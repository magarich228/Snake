using System;

namespace Snake
{
    class SnakeTail : BaseObject
    {
        private BaseObject baseSnakeElement;

        public MoveTail MoveSubsequentItem { get; set; }

        public BaseObject BaseSnakeElement
        {
            get => baseSnakeElement;

            set
            {
                if (value.TypeOb != Type.Snake & value.TypeOb != Type.SnakeTail)
                    throw new ArgumentException("В конструкторе SnakeTail произошло присваивание не того типа.");

                baseSnakeElement = value;
            }
        }

        public int X
        {
            get => x;

            set => x = value;
        }
        public int Y
        {
            get { return y; }

            set { y = value; }
        }

        override public Type TypeOb { get => Type.SnakeTail; }

        public SnakeTail() : base("██") { }

        public void Initialize(BaseObject snakeElement)
        {
            BaseSnakeElement = snakeElement;

            BaseObject tempObject = baseSnakeElement;

            while (tempObject.TypeOb != Type.Snake)
            {
                tempObject = tempObject.TypeOb == Type.SnakeTail ? ((SnakeTail)tempObject).BaseSnakeElement : tempObject;
            }

            int index;
            bool detected = default;

            for (index = 0; index < ((Snake)tempObject).snakeTails.Length; index++)
            {
                if (((Snake)tempObject).snakeTails[index] != null && ((Snake)tempObject).snakeTails[index].GetHashCode() == this.GetHashCode() //((Snake)tempObject).snakeTails[index] != null && 
                    && index != ((Snake)tempObject).snakeTails.Length - 1)
                {
                    detected = true;
                    index++;

                    break;
                }
            }

            MoveSubsequentItem = detected ? ((Snake)tempObject).snakeTails[index].Move : MoveSubsequentItem;
        }

        public void Move(int Y, int X)
        {
            if (MoveSubsequentItem != null)
                MoveSubsequentItem(this.Y, this.X);

            this.X = X;
            this.Y = Y;
        }
    }
}
