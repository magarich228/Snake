using System;
using System.IO;
using System.Threading;

namespace Snake
{
    class Map
    {
        private const string Path = "C:\\Users\\kiril\\source\\repos\\Snake\\Snake\\DATA\\";      //В класс Maps + Переопределить для Maps своё исключения выхода за границы в свойствах
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public string Name { get; set; }

        public Matrix matrix;

        public Map(int y, int x, string name, bool New)
        {
            SizeX = x;
            SizeY = y;

            Name = name;

            matrix = new Matrix(y, x, this);

            if (New)
                this.ToPreloadingFile();
        }

        public Map() { }

        public string GetPath() => Path + Name;

        public const string PathToPreloadingFile = "C:\\Users\\kiril\\source\\repos\\Snake\\Snake\\DATA\\PreloadingAllMapData.txt";
        private void ToPreloadingFile()
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = default;

            try
            {
                fileStream = new FileStream(PathToPreloadingFile, FileMode.Open, FileAccess.Write);
                streamWriter = new StreamWriter(fileStream);

                fileStream.Seek(0, SeekOrigin.End);

                streamWriter.Write(this.Name + "\n" + this.SizeX +
                    "\n" + this.SizeY + "\n" + this.GetPath() + "\n.1" + "\n.2\n");//Line + \n

                streamWriter.Flush();
            }

            catch (IOException)
            {
                Console.WriteLine("Исключение IO типа");
                Thread.Sleep(1500);
            }

            finally
            {
                streamWriter?.Close();

                fileStream?.Close();
            }
        }
    }
}
