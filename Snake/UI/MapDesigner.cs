using System;
using System.IO;
using System.Threading;

namespace Snake
{
    class MapDesigner
    {
        private static MapDesigner mapDesigner;

        private static bool endDesing;

        private MapDesigner() { }

        static MapDesigner()
        {
            mapDesigner = new MapDesigner();
            endDesing = false;
        }

        public void Launch(string name) => ShowMenu(false, name);
        public void Launch(bool New) => ShowMenu(New, "Empty");

        public static MapDesigner GetComponent() => mapDesigner;

        private void ShowMenu(bool New, string name)
        {
            string NameForRender = name;
            
            if (New)
            {
                Map map = default;

                Console.Clear();

                Console.Write("Size of Map Y x X - ");

                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.Write($"Max Map Size - {Console.LargestWindowHeight - 1} x {112}");  // 112 - ||
                try                                                                          //       \/
                {
                    Console.CursorVisible = true;

                    Console.SetCursorPosition(20, 0);
                    int SizeY = int.Parse(Console.ReadLine());

                    Console.SetCursorPosition(20 + SizeY.ToString().Length, Console.CursorTop - 1);
                    Console.Write(" x ");

                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                    int SizeX = int.Parse(Console.ReadLine());

                    if (SizeX < 10 || SizeY < 13 ||
                        SizeX + 16 > 112 + 16 || SizeY + 1 > Console.LargestWindowHeight)   // 112 - LargestConsoleWidth not breaking the code
                    {
                        Console.Clear();
                        Console.WriteLine("Unacceptable map size!");

                        Thread.Sleep(2000);

                        ShowMenu(New, name);
                        return;
                    }

                    Console.Write("Name - ");
                    string Name = Console.ReadLine();

                    for (int countMap = 0; countMap < GameComponents.GetLastIndex(); countMap++)
                    {
                        if (GameComponents.GetLastIndex() > 0 && GameComponents.Maps[countMap].Name == Name)
                        {
                            Console.Clear();
                            Console.WriteLine("Map with this name already exist!");

                            Thread.Sleep(2000);

                            ShowMenu(New, name);
                            return;
                        }
                    }

                    map = new Map(SizeY, SizeX, Name, true);

                    using (FileStream fileStream = new FileStream(map.GetPath(), FileMode.CreateNew, FileAccess.ReadWrite)) { }
                    using (FileStream fileStream = new FileStream(map.GetPath() + "Designer", FileMode.CreateNew, FileAccess.ReadWrite)) { }
                }
                catch (FormatException) { ShowMenu(true, "Empty"); }

                catch (OverflowException) { ShowMenu(true, "Empty"); }

                catch (IOException) 
                {
                    Console.WriteLine("IOException catch");
                    ShowMenu(true, "Empty"); 
                }

                GameComponents.Maps[GameComponents.GetLastIndex()] = map;

                NameForRender = map.Name;

            }

            for (int index = 0; index < GameComponents.GetLastIndex(); index++)
            {
                if (GameComponents.Maps[index].Name == NameForRender)
                {
                    int Left = 0, Top = 0;
                    endDesing = false;

                    int count = 0;
                    while (!endDesing)
                    {
                        DesignerMenu(index, Left, Top, count != 0 ? ToFile.ToDesigner : ToFile.ToSave);
                        ReadMenu(index, ref Left, ref Top);

                        Save(index, Left, Top, ToFile.ToDesigner);

                        count++;
                    }

                    GameComponents.Maps[index].matrix.ClearMatrix();
                    Save(index, Left, Top, ToFile.ToDesigner);
                    
                }
            }

            Preservation.GetComponent().Launch();
        }

        private void ReadMenu(int index, ref int Left, ref int Top)
        {
            ConsoleKeyInfo consoleKeyInfo = default;

            if (Console.KeyAvailable)
            {
                consoleKeyInfo = Console.ReadKey(true);

                while (Console.KeyAvailable)
                    Console.ReadKey(true);
            }

            Thread.Sleep(50);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.W:Top = Top - 1 >= 0 ? --Top : Top;
                    break;

                case ConsoleKey.A:
                    Left = Left - 2 >= 0 ? Left - 2 : Left;
                    break;

                case ConsoleKey.S:
                    Top = Top + 1 < GameComponents.Maps[index].matrix.GetLong(Axis.y) ? ++Top : Top;
                    break;

                case ConsoleKey.D:
                Left = Left + 2 < GameComponents.Maps[index].matrix.GetLong(Axis.x) * 2 ? Left + 2 : Left;
                    break;

                case ConsoleKey.UpArrow:
                    goto case ConsoleKey.W;

                case ConsoleKey.DownArrow:
                    goto case ConsoleKey.S;

                case ConsoleKey.LeftArrow:
                    goto case ConsoleKey.A;

                case ConsoleKey.RightArrow:
                    goto case ConsoleKey.D;

                case ConsoleKey.F:

                    GameComponents.Maps[index].matrix[Console.CursorTop, Console.CursorLeft / 2] =
                        GameComponents.Maps[index].matrix[Console.CursorTop, Console.CursorLeft / 2] == null ? new Wall() : null;

                    break;

                case ConsoleKey.Enter:
                    goto case ConsoleKey.F;

                case ConsoleKey.B:

                    int ind;
                    int line;

                    bool detectedBox = true;

                    for (line = 0; line < GameComponents.Maps[index].matrix.GetLong(Axis.y); line++)
                    {
                        if (line == 0 || line == GameComponents.Maps[index].matrix.GetLong(Axis.y) - 1)
                        {
                            ind = 0;
                            while (ind < GameComponents.Maps[index].matrix.GetLong(Axis.x))
                            {
                                detectedBox = GameComponents.Maps[index].matrix[line, ind] != null ? detectedBox : false;
                                ind++;
                            }

                            continue;
                        }

                        detectedBox = GameComponents.Maps[index].matrix[line, 0] != null ? detectedBox : false;
                        detectedBox = GameComponents.Maps[index].matrix[line, GameComponents.Maps[index].matrix.GetLong(Axis.x) - 1] != null ? detectedBox : false;
                    }


                    object element = detectedBox ? null : new Wall();

                    for (line = 0; line < GameComponents.Maps[index].matrix.GetLong(Axis.y); line++)
                    {
                        if (line == 0 || line == GameComponents.Maps[index].matrix.GetLong(Axis.y) - 1)
                        {
                            ind = 0;
                            while (ind < GameComponents.Maps[index].matrix.GetLong(Axis.x))
                            {
                                GameComponents.Maps[index].matrix[line, ind] = (BaseObject)element;
                                ind++;
                            }

                            continue;
                        }

                        GameComponents.Maps[index].matrix[line, 0] = (BaseObject)element;
                        GameComponents.Maps[index].matrix[line, GameComponents.Maps[index].matrix.GetLong(Axis.x) - 1] = (BaseObject)element;
                    }

                    break;

                case ConsoleKey.C:
                    GameComponents.Maps[index].matrix.ClearMatrix();
                    break;

                case ConsoleKey.Escape:
                    End(index);
                    break;

                case ConsoleKey.F1:
                    Save(index, Left, Top, ToFile.ToSave);
                    break;

                default:
                    break;
            }
        }
        private static void Save(int index, int PosLeft, int PosTop, ToFile toFile)
        {
            string path = (toFile == ToFile.ToSave) ? GameComponents.Maps[index].GetPath() : GameComponents.Maps[index].GetPath() + "Designer";

            StreamWriter streamWriter = default;
            FileStream fileStream = default;

            try
            {
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Write);
                streamWriter = new StreamWriter(fileStream);

                string buffer = default;

                for (int CorY = 0; CorY < GameComponents.Maps[index].matrix.GetLong(Axis.y); CorY++)
                {
                    for (int CorX = 0; CorX < GameComponents.Maps[index].matrix.GetLong(Axis.x); CorX++)
                    {
                        buffer += GameComponents.Maps[index].matrix[CorY, CorX] != null ? GameComponents.Maps[index].matrix[CorY, CorX].symbol : "  ";
                    }
                    buffer += "\n";
                }

                streamWriter.Write(buffer);
            }
            catch (IOException ob) 
            {
                Console.WriteLine(ob.Message);
                Thread.Sleep(3000);
            }
            catch (Exception ob)
            {
                Console.WriteLine(ob.Message);
                Thread.Sleep(3000);
            }

            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Dispose();
                    streamWriter.Close();
                }

                if (fileStream != null)
                {
                    fileStream.Dispose();
                    fileStream.Close();
                }
            }

            int PosX = Console.CursorLeft, PosY = Console.CursorTop;

            if (toFile == ToFile.ToSave)
            {
                Console.SetCursorPosition(Console.WindowWidth - 8, Console.WindowHeight - 1);
                Console.Write("Saved");

                Console.SetCursorPosition(PosLeft, PosTop);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
        private void DesignerMenu(int index, int PosLeft, int PosTop, ToFile toFile)
        {
            Console.Clear();

            Console.WindowHeight = GameComponents.Maps[index].matrix.GetLong(Axis.y) + 1;
            Console.WindowWidth = GameComponents.Maps[index].matrix.GetLong(Axis.x) * 2 + 16;

            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            GameComponents.Maps[index].matrix.DownloadFromFile(toFile);
            

            Render.GetComponent().MatrixRender(GameComponents.Maps[index].matrix, Console.CursorLeft, Console.CursorTop);

            Console.SetCursorPosition(Console.WindowWidth - 15, 0);
            Console.Write("INSRUMENTS:");

            Console.SetCursorPosition(Console.WindowWidth - 15, 1);
            Console.Write("enter, f - ");

            Console.SetCursorPosition(Console.WindowWidth - 15, 2);
            Console.Write("put/delete wall");

            Console.SetCursorPosition(Console.WindowWidth - 15, 3);
            Console.Write("b - put/delete");

            Console.SetCursorPosition(Console.WindowWidth - 15, 4);
            Console.Write("wall box");

            Console.SetCursorPosition(Console.WindowWidth - 15, 5);
            Console.Write("c - clear map");

            Console.SetCursorPosition(Console.WindowWidth - 15, 6);
            Console.Write("F1 - save");

            Console.SetCursorPosition(Console.WindowWidth - 15, 7);
            Console.Write("esc - exit");

            Console.CursorVisible = true;
            Console.SetCursorPosition(PosLeft, PosTop);
        }

        private static void End(int index) => endDesing = true;
    }

    enum ToFile
    {
        ToSave,
        ToDesigner
    }
}
