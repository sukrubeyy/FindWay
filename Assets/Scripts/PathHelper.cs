    using UnityEngine;

    public static class PathHelper
    {
        public static class Path
        {
            public static string CustomizationFolderPath = Application.dataPath + "/Customization/";
            public static string LevelsPath = Application.dataPath + "/Levels/";
        }
        
        public static class FileName
        {
            public static string CustomizationJsonName = "CustomizationSettings.json";
        }
    }
