using System;

namespace Snake
{
    delegate void Move();
    delegate void MoveTail(int y, int x);

    class Snake : BaseObject
    {
        public SnakeTail[] snakeTails;

        IController control;
        Direction direction;

        event MoveTail MoveSnakeTail;

        int[] EndElementXY = new int[2];

        public IController Control
        {
            get => control;
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

        public Direction Direction
        {
            get => direction;

            set => direction = Ok((int)value) ?
                value : ((int)value == -1) ? Direction.right : Direction.up;
        }

        override public Type TypeOb
        {
            get => Type.Snake;
        }

        public Snake(string head, IController control) : base(head)
        {
            this.control = control;
            snakeTails = new SnakeTail[3];

            for (int index = 0; index < 3; index++)
            {
                snakeTails[index] = new SnakeTail();
            }

            for (int count = 0; count < 3; count++)
            {
                snakeTails[count].Initialize(count != 0 ? snakeTails[count - 1] : this);
            }

            MoveSnakeTail = snakeTails[0].Move;

            control.Snake = this;
        }

        public void Move()
        {
            EndElementXY[0] = snakeTails[snakeTails.Length - 1].X;
            EndElementXY[1] = snakeTails[snakeTails.Length - 1].Y;

            Grow(0);

            switch (direction)
            {
                case Direction.up:

                    Y--;
                    MoveSnakeTail(Y + 1, X);

                    break;

                case Direction.left:

                    X--;
                    MoveSnakeTail(Y, X + 1);

                    break;

                case Direction.down:

                    Y++;
                    MoveSnakeTail(Y - 1, X);

                    break;

                case Direction.right:

                    X++;
                    MoveSnakeTail(Y, X - 1);

                    break;
            }
        }

        private int GrowCount = default;

        public void Grow(int numOfElement)
        {
            if (GrowCount > 0)
            {
                Array.Resize(ref snakeTails, snakeTails.Length + 1);

                snakeTails[snakeTails.Length - 1] = new SnakeTail();
                snakeTails[snakeTails.Length - 1].BaseSnakeElement = snakeTails[snakeTails.Length - 2];

                snakeTails[snakeTails.Length - 1].X = EndElementXY[0];
                snakeTails[snakeTails.Length - 1].Y = EndElementXY[1];

                Engine.ThisGameSettings.Map.matrix[snakeTails[snakeTails.Length - 1].Y, snakeTails[snakeTails.Length - 1].X]
                    = snakeTails[snakeTails.Length - 1];

                snakeTails[snakeTails.Length - 2].MoveSubsequentItem = snakeTails[snakeTails.Length - 1].Move;

                GrowCount--;
            }

            GrowCount += numOfElement;
        }

        public void Death()
        {
            if (GameComponents.Player != null && this.GetHashCode() == GameComponents.Player.GetHashCode())
            {
                for (int tailIndex = 0; tailIndex < GameComponents.Player.snakeTails.Length; tailIndex++)
                {
                    GameComponents.Player.snakeTails[tailIndex] = null;
                    Engine.AllSnake[Engine.AllSnake.Length - 1].snakeTails[tailIndex] = null;
                }

                GameComponents.Player = null;
                Engine.AllSnake[Engine.AllSnake.Length - 1] = null;
            }

            for (int index = 0; index < Engine.GetComponent().CountSnake(GameComponents.Bots); index++)
            {
                if (GameComponents.Bots[index] != null && GameComponents.Bots[index].GetHashCode() == this.GetHashCode())
                {
                    for (int tailIndex = 0; tailIndex < GameComponents.Bots[index].snakeTails.Length; tailIndex++)
                    {
                        GameComponents.Bots[index].snakeTails[tailIndex] = null;
                        Engine.AllSnake[index].snakeTails[tailIndex] = null;
                    }

                    GameComponents.Bots[index] = null;
                    Engine.AllSnake[index] = null;
                }
            }

            Engine.RemoveMoveEvent(this.Move);
        }

        private static bool Ok(int direction) => direction >= 0 && direction <= 3;
    }

    enum Direction
    {
        up,
        left,
        down,
        right
    }
}