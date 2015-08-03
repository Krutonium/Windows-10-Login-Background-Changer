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
            switch (args[0])
            {
                case "authors":
                    Console.WriteLine("");
                    Console.WriteLine(@"Syrexide | https://github.com/Syrexide/");
                    Console.WriteLine(@"Toyz | https://github.com/Toyz/");
                    return;

                case "restore":
                    Restore();
                    return;
            }

            if (args.Length == 1)
            {
                var filedir = args[0];
                if (File.Exists(filedir) && (Path.GetExtension(filedir) == ".jpg") || (Path.GetExtension(filedir) == ".png") || (Path.GetExtension(filedir) == ".bmp") || (Path.GetExtension(filedir) == ".tif") || (Path.GetExtension(filedir) == ".tiff"))
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
                Console.WriteLine(@"Please format your command as shown: commandthing [imagepath]");
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

        private static void Restore()
        {
            HelperLib.TakeOwnership(Config.PriFileLocation);
            File.Copy(Config.BakPriFileLocation, Config.PriFileLocation, true);
        }
    }
}