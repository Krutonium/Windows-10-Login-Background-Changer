using HelperLibrary;
using System;
using System.IO;
using W10_Logon_BG_Changer___Command_Line.Tools;

namespace W10_Logon_BG_Changer___Command_Line
{
    internal class Program
    {
        private readonly static string NewPriLocation = Path.Combine(Path.GetTempPath(), "new_temp_pri.pri");
        private readonly static string TempPriFile = Path.Combine(Path.GetTempPath(), "bak_temp_pri.pri");

        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var filedir = args[0];
                if (File.Exists(filedir) && (Path.GetExtension(filedir) == ".jpg"))
                {
                    ChangeImage(filedir);
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine(@"File ""{0}"" does not exist, or the file is not an image file!", filedir);
                }
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine(@"Please format your command as shown: commandthing [image path]");
                Console.WriteLine("");
                Console.WriteLine(@"Press any key to exit.");
                Console.ReadKey();
            }
        }

        private static void ChangeImage(string filedir)
        {
            HelperLib.TakeOwnership(Config.LogonFolder);
            HelperLib.TakeOwnership(Config.PriFileLocation);

            if (!File.Exists(Config.BakPriFileLocation))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(@"[Warning]: Could not find Windows.UI.Logon.pri.bak file. Creating new.");
                Console.ForegroundColor = ConsoleColor.White;
                File.Copy(Config.PriFileLocation, Config.BakPriFileLocation);
            }

            HelperLib.TakeOwnership(Config.BakPriFileLocation);

            File.Copy(Config.BakPriFileLocation, TempPriFile, true);

            if (File.Exists(NewPriLocation))
            {
                File.Delete(NewPriLocation);
            }

            File.Copy(Config.BakPriFileLocation, TempPriFile, true);

            PriBuilder.CreatePri(TempPriFile, NewPriLocation, filedir);

            File.Copy(NewPriLocation, Config.PriFileLocation, true);
        }
    }
}