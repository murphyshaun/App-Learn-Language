using System;
using System.IO;

namespace Common.Extension
{
    public static class EnvironmentPath
    {
        private static string CurrentParentPath = Environment.CurrentDirectory;

        public static string PathImageStore = Path.Combine(CurrentParentPath, @"Store\");


        public static string PathFileDatabase = Path.Combine(CurrentParentPath, @"Managers\EnglishNewWord.db");
    }
}
