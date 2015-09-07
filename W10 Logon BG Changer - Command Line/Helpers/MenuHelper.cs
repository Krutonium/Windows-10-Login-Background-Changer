using System;

namespace W10_Logon_BG_Changer___Command_Line.Helpers
{
    internal class MenuHelper
    {
        public static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("HELP");
            Console.WriteLine("=====");
            Console.WriteLine();
            Console.WriteLine("AUTHORS - /A");
            Console.WriteLine("COLOR - /C [HEX CODE]");
            Console.WriteLine("IMAGE - /I [IMAGE PATH]");
            Console.WriteLine("RESTORE - /R");
            Console.WriteLine("HELP - /H");
        }

        public static void ShowAuthors()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine(@"Syrexide | https://github.com/Syrexide/");
            Console.WriteLine(@"Toyz | https://github.com/Toyz/");
            Console.ResetColor();
        }

        public static void ShowSuccessMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Success!");
            Console.ResetColor();
        }
    }
}
