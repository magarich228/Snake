using System;
using System.IO;
using System.Threading;

namespace Snake
{
    class Matrix
    {
        BaseObject[,] matrix;

        Map MapOfMatrix;

        public BaseObject this[int y, int x]
        {
            get
            {
                int Y = y, X = x;

                if (!this.Ok(y, Axis.y))
                    Y = y - (y < 0 ? -(int)matrix.GetLength((int)Axis.y): (int)matrix.GetLength((int)Axis.y));

                if (!this.Ok(x, Axis.x))
                    X = x - (x < 0 ? -(int)matrix.GetLength((int)Axis.x): (int)matrix.GetLength((int)Axis.x));

                return matrix[Y, X];
            }
            set
            {
                int Y = y, X = x;   

                if (!this.Ok(y, Axis.y))
                    Y = y -  (y < 0 ? -(int)matrix.GetLength((int)Axis.y): (int)matrix.GetLength((int)Axis.y));

                if (!this.Ok(x, Axis.x))
                    X = x - (x < 0 ? -(int)matrix.GetLength((int)Axis.x) : (int)matrix.GetLength((int)Axis.x));

                if (value == null || value.TypeOb == Type.Wall || value.TypeOb == Type.Snake ||
                    value.TypeOb == Type.SnakeTail || value.TypeOb == Type.Bonus || value.TypeOb == Type.SpecialBonus)
                {
                    matrix[Y, X] = value;
                }
                else
                    throw new Exception("Unsuitable type");
            }
        }

        public Matrix(int y, int x) => matrix = new BaseObject[y, x];

        public Matrix(int y, int x, Map map) : this(y, x) => MapOfMatrix = map;

        private bool Ok(int coordinate, Axis axis) =>
            coordinate < matrix.GetLongLength((int)axis) & coordinate >= 0;


        public int GetLong() => matrix.Length;

        public int GetLong(Axis axis) =>
            axis == 0 ? (int)matrix.GetLongLength(0) : (int)matrix.GetLongLength(1);


        public bool DownloadFromFile(ToFile toFile)
        {
            string path = (toFile == ToFile.ToSave) ? MapOfMatrix.GetPath() : MapOfMatrix.GetPath() + "Designer";

            FileStream fileStream = default;
            StreamReader streamReader = default;

            bool NotError = true;

            try
            {
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                streamReader = new StreamReader(fileStream);

                char[] buffer = new char[2];

                for (int CorY = 0; CorY < this.GetLong(Axis.y); CorY++)
                {
                    for (int CorX = 0; CorX < this.GetLong(Axis.x); CorX++)
                    {
                        if (streamReader.ReadBlock(buffer, 0, 2) != 2)
                            //throw new Exception("Not all characters are read successfully");
#pragma warning disable CS0642
                            ;
#pragma warning restore CS0642

                        matrix[CorY, CorX] = buffer[0] == '╬' && buffer[1] == '╬' ? new Wall() : null;
                    }

                    streamReader.ReadLine();
                }
            }

            catch (IOException ob)
            {
                Console.WriteLine(ob.Message, ob.StackTrace);
                Thread.Sleep(3000);

                return false;
            }

            catch (Exception ob) { Console.WriteLine(ob.Message); return false; }

            finally
            {
                    streamReader?.Close();
                    fileStream?.Close();
            }

            return NotError;
        }

        public void ClearMatrix()
        {
            for (int CorY = 0; CorY < GetLong(Axis.y); CorY++)
            {
                for (int CorX = 0; CorX < GetLong(Axis.x); CorX++)
                {
                    this[CorY, CorX] = null;
                }
            }
        }

        public void ClearMatrixOfSnakes()
        {
            for (int CorY = 0; CorY < GetLong(Axis.y); CorY++)
            {
                for (int CorX = 0; CorX < GetLong(Axis.x); CorX++)
                {
                    this[CorY, CorX] = this[CorY, CorX] != null ? 
                        this[CorY, CorX].TypeOb == Type.Snake || this[CorY, CorX].TypeOb == Type.SnakeTail ? null : this[CorY, CorX] : null;
                }
            }
        }
    }
    enum Axis
    {
        y = 0,
        x = 1
    }
}
