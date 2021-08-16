using System;

namespace Snake
{
    class GameSettings
    {
        public int NumOfBots { get; set; }
        public bool PlayerExist { get; set; }
        public Map Map { get; set; }
        public bool DynamicComplexity { get; set; }
        public int GameTempo { get; set; }

        private static GameSettings gameSettings;

        static GameSettings() => gameSettings = new GameSettings();

        private GameSettings() 
        {
            NumOfBots = 0;
            PlayerExist = true;
            Map = default;
            DynamicComplexity = false;
            GameTempo = 500;
        }

        public static GameSettings GetSettings() => gameSettings;

        public static void RefreshSettings() => gameSettings = new GameSettings();
    }
}
