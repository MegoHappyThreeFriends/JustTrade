using System;

namespace JustTrade.Helpers
{
    public static class FileUtils
    {
        public static bool isValidFileName (string fileName)
        {
            string invalidChars = ".\\/:*&?'<>|\"";
            for (int i = 0; i < fileName.Length; i++) {
                bool exist = false;
                for (int j = 0; j < invalidChars.Length; j++) {
                    if (fileName [i] == invalidChars [j]) 
                    {
                        exist = true;
                    }
                }
                if (!exist) {
                    return false;
                }
            }
            return true;
        }
    }
}

