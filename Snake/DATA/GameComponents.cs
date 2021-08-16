using System;
using System.Threading;
using System.IO;

namespace Snake
{
    struct GameComponents
    {
        public static Map[] Maps;

        public static Snake Player;

        public static Snake[] Bots;

        static GameComponents()
        {
            Maps = new Map[10];

            RemoveNullInPreloading();
            RemoveNullInPreloading();

            long line = 0;
            for (int index = 0; index < Maps.Length; index++)
                if (FromPreloading(ref Maps[index], ref line))
                    break;
        }

        public static int GetLastIndex()
        {
            for (int index = 0; index < Maps.Length; index++)
                if (Maps[index] == null)
                    return index;

            return 10;
        }

        private static bool FromPreloading(ref Map map, ref long Line) 
        {
            FileStream fileStream = default;
            StreamReader streamReader = default;        

            try
            {
                fileStream = new FileStream(Map.PathToPreloadingFile, FileMode.Open, FileAccess.Read);
                streamReader = new StreamReader(fileStream);

                string enter;

                for (int line = 0; line < Line; line++)
                    enter = streamReader.ReadLine();

                string Name = streamReader.ReadLine();  
                int SizeX = int.Parse(streamReader.ReadLine());
                int SizeY = int.Parse(streamReader.ReadLine());

                Line += 6;
                
                map = new Map(SizeY, SizeX, Name, false);
            }
            catch (IOException) { return true; }
            catch (ArgumentNullException) { return true; }

            finally
            {
                if (streamReader != null)
                    streamReader.Close();

                if (fileStream != null)
                    fileStream.Close();
            }

            return false;
        }

        public static void RemoveMap(int index)
        {
            FileStream fileStream = default;

            StreamReader streamReader = default;
            StreamWriter streamWriter = default;

            StringWriter stringWriter = default;
            StringReader stringReader = default;

            string buffer;

            try
            {
                File.Delete(Maps[index].GetPath());
                File.Delete(Maps[index].GetPath() + "Designer");

                fileStream = new FileStream(Map.PathToPreloadingFile, FileMode.Open, FileAccess.Read);
                streamReader = new StreamReader(fileStream);
                stringWriter = new StringWriter();

                buffer = "Go";

                while (buffer != null)
                {
                    buffer = streamReader.ReadLine();

                    if (buffer == Maps[index].Name)
                    {
                        for (int iteration = 0; iteration < 5; iteration++)
                            streamReader.ReadLine();
                    }
                    else
                        stringWriter.WriteLine(buffer);
                }

                fileStream.Close();
                streamReader.Dispose();

                fileStream = new FileStream(Map.PathToPreloadingFile, FileMode.Create, FileAccess.Write);
                streamWriter = new StreamWriter(fileStream);
                stringReader = new StringReader(stringWriter.ToString());

                stringWriter.Dispose();

                streamWriter.WriteLine(stringReader.ReadToEnd());
                streamWriter.Flush();
            }
            catch (IOException ob) { Console.WriteLine(ob.Message); }
            catch (Exception ob) { Console.WriteLine(ob.Message); }

            finally
            {
                if (streamReader != null)
                    streamReader.Close();

                if (streamWriter != null)
                    streamWriter.Close();

                if (stringWriter != null)
                    stringWriter.Close();

                if (stringReader != null)
                    stringReader.Close();

                if (fileStream != null)
                    fileStream.Close();
            }

            RemoveNullInPreloading();
            RemoveNullInPreloading();

            Maps[index] = null;

            Map[] MapsCopy = new Map[Maps.Length];

            Maps.CopyTo(MapsCopy, 0);

            Maps = new Map[MapsCopy.Length];

            for (int MapsIndex = index; MapsIndex < Maps.Length - 1; MapsIndex++)
            {
                MapsCopy[MapsIndex] = MapsCopy[MapsIndex + 1];
            }

            for (int MapsIndex = 0; MapsIndex < Maps.Length - 1; MapsIndex++)
            {
                Maps[MapsIndex] = MapsCopy[MapsIndex];
            }
        }
        private static void RemoveNullInPreloading()
        {
            FileStream fileStream = default;

            StreamReader streamReader = default;
            StreamWriter streamWriter = default;

            StringWriter stringWriter = default;
            StringReader stringReader = default;

            string buffer;

            try
            {
                fileStream = new FileStream(Map.PathToPreloadingFile, FileMode.Open, FileAccess.Read);
                streamReader = new StreamReader(fileStream);
                stringWriter = new StringWriter();

                while (!streamReader.EndOfStream)
                {
                    buffer = streamReader.ReadLine();

                    if (buffer != "")
                        stringWriter.WriteLine(buffer);
                }

                fileStream.Close();
                streamReader.Close();

                fileStream = new FileStream(Map.PathToPreloadingFile, FileMode.Create, FileAccess.Write);
                streamWriter = new StreamWriter(fileStream);
                stringReader = new StringReader(stringWriter.ToString());

                stringWriter.Dispose();

                streamWriter.WriteLine(stringReader.ReadToEnd());
                streamWriter.Flush();
            }
            catch (IOException ob) 
            { 
                Console.WriteLine(ob.Message + " in GameConponents-RemoveNull");
                Thread.Sleep(3000);
            }
            catch (Exception ob) { Console.WriteLine(ob.Message); Thread.Sleep(3000); }

            finally
            {
                if (streamReader != null)
                    streamReader.Close();

                if (streamWriter != null)
                    streamWriter.Close();

                if (stringWriter != null)
                    stringWriter.Close();

                if (stringReader != null)
                    stringReader.Close();

                if (fileStream != null)
                    fileStream.Close();
            }
        }
    }
}
