using System;

namespace Snake
{
    delegate void WithDirection(Direction direction);

    class AI : IController
    {
        private Snake _baseSnake;
        public Snake Snake
        {
            get => _baseSnake;

            set
            {
                if (value != null && value.TypeOb != Type.Snake)
                    throw new FormatException("тип не является типом Snake");

                _baseSnake = value;
            }
        }

        private static Matrix matrix;

        private static Random rnd;

        public AIState State { get; private set; }


        event WithDirection SendCommand;

        static AI()
        {
            matrix = GameSettings.GetSettings().Map.matrix;
            rnd = new Random();
        }

        public AI(Snake baseSnake)
        {
            this.Snake = baseSnake;

            SendCommand = (Direction direction) => Snake.Direction = direction;
        }

        public void Read()
        {
            BaseObject toThisBonus = default;
            double lengthThatOb = double.MaxValue;

            foreach (BaseObject bonus in Engine.GameGenerate.Bonuses)
            {
                if (bonus as Bonus != null)
                {
                    double length =
                        Math.Sqrt((((Bonus)bonus).X - _baseSnake.X)
                        * (((Bonus)bonus).X - _baseSnake.X)
                        + (((Bonus)bonus).Y - _baseSnake.Y)
                        * (((Bonus)bonus).Y - _baseSnake.Y));

                    toThisBonus = length < lengthThatOb ? bonus : toThisBonus;
                    lengthThatOb = length < lengthThatOb ? length : lengthThatOb;
                }
                else if (bonus != null)
                {
                    double length =
                        Math.Sqrt((((SpecialBonus)bonus).X - _baseSnake.X)
                        * (((SpecialBonus)bonus).X - _baseSnake.X)
                        + (((SpecialBonus)bonus).Y - _baseSnake.Y)
                        * (((SpecialBonus)bonus).Y - _baseSnake.Y));

                    toThisBonus = length < lengthThatOb ? bonus : toThisBonus;
                    lengthThatOb = length < lengthThatOb ? length : lengthThatOb;
                }

            }

            Snake toThisSnake = default;
            double lengthThatSn = double.MaxValue;

            foreach (Snake snake in Engine.AllSnake)
            {
                if (object.Equals(snake, _baseSnake))
                    continue;

                double length =
                        Math.Sqrt((snake.X - _baseSnake.X) * (snake.X - _baseSnake.X)
                        + (snake.Y - _baseSnake.Y) * (snake.Y - _baseSnake.Y));

                toThisSnake = length < lengthThatSn ? snake : toThisSnake;
                lengthThatSn = length < lengthThatSn ? length : lengthThatSn;
            }

            int[] endCoordinate = ChooseCoordinate(lengthThatSn, toThisSnake, lengthThatOb, toThisBonus);

            Coordinate[] Way = SearchBestWay(endCoordinate[1], endCoordinate[0]);

            Coordinate nextCoordinate = Way.Length >= 2 ? Way[1] : new Coordinate
                (_baseSnake.X + _baseSnake.Direction == Direction.up? 0 : _baseSnake.Direction == Direction.down ? 
                0 : _baseSnake.Direction == Direction.left ? -1 : _baseSnake.Direction == Direction.right ? 1 : _baseSnake.X,
                _baseSnake.Y + _baseSnake.Direction == Direction.up ? -1 : _baseSnake.Direction == Direction.down ? 
                1 : _baseSnake.Direction == Direction.left ? 0: _baseSnake.Direction == Direction.right ? 0 : _baseSnake.Y);

            _baseSnake.Direction = nextCoordinate.Y == _baseSnake.Y - 1 ? Direction.up : nextCoordinate.Y == _baseSnake.Y + 1 ?
                Direction.down : nextCoordinate.X == _baseSnake.X - 1 ? Direction.left : nextCoordinate.X == _baseSnake.X + 1 ?
                Direction.right : _baseSnake.Direction;
        }

        private void SetState(AIState State) => this.State = State;

        private int[] ChooseCoordinate(double lengthToSn, Snake snake, double legthToOb, BaseObject bonus)
        {
            if (snake != null && lengthToSn < legthToOb / 3)
            {
                SetState(AIState.Attack);

                int[] coor = new int[2] 
                { snake.Direction == Direction.down? snake.X : snake.Direction == Direction.up?
                snake.X : snake.Direction == Direction.left? snake.X - 1 : snake.Direction == Direction.right?
                snake.X + 1 : snake.X, snake.Direction == Direction.down ? snake.Y + 1 : snake.Direction == Direction.up ?
                snake.Y - 1: snake.Direction == Direction.left ? snake.Y : snake.Direction == Direction.right ? snake.Y : snake.Y };

                if (matrix[coor[1], coor[0]] != null && bonus != null)
                {
                    SetState(AIState.ToBonus);

                    return new int[2]
                    { bonus is Bonus ? ((Bonus)bonus).X : ((SpecialBonus)bonus).X,
                      bonus is Bonus ? ((Bonus)bonus).Y : ((SpecialBonus)bonus).Y };
                }

                return coor;
            }
            else if (bonus != null)
            {
                int[] Out = new int[2];

                Out[0] = bonus is Bonus ? ((Bonus)bonus).X : ((SpecialBonus)bonus).X;
                Out[1] = bonus is Bonus ? ((Bonus)bonus).Y : ((SpecialBonus)bonus).Y;

                SetState(AIState.ToBonus);
                return Out;
            }
            
            return new int[2] { 0, 0 };
        }

        private Coordinate[] SearchBestWay(int Y2, int X2)
        {
            if (_baseSnake.Y == Y2 && _baseSnake.X == X2) //
                return new Coordinate[2]
                {
                    new Coordinate(_baseSnake.X, _baseSnake.Y),

                    new Coordinate(_baseSnake.X + _baseSnake.Direction == Direction.up ? 0 : _baseSnake.Direction == Direction.down ?
                0 : _baseSnake.Direction == Direction.left ? -1 : _baseSnake.Direction == Direction.right ? 1 : 0,

                _baseSnake.Y + _baseSnake.Y + _baseSnake.Direction == Direction.up ? -1 : _baseSnake.Direction == Direction.down ?
                1 : _baseSnake.Direction == Direction.left ? 0 : _baseSnake.Direction == Direction.right ? 0 : 0)
                };

            StructCoordinate Struct = new StructCoordinate(_baseSnake.X, _baseSnake.Y);
            LinkedGroup endElement = default;

            while (endElement == null)
            {
                Struct.Add();
                endElement = Struct.Check(Y2, X2);
            }

            Coordinate[] Way = new Coordinate[1];
            Way[0] = endElement.Set;

            while (true)
            {
                Coordinate tempCor = endElement?.Base;

                if (tempCor == null)
                    break;

                Array.Resize(ref Way, Way.Length + 1);

                Way[Way.Length - 1] = tempCor;

                endElement = Struct.GetLinks(tempCor);
            }

            Array.Reverse(Way);

            return Way;
        }
    }

    enum AIState
    {
        ToBonus,
        Attack,
        Savely
    }
}
