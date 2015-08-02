using HelperLibrary;
using System;
using System.IO;
using W10_BG_Logon_Changer;
using W10_BG_Logon_Changer.Tools;

namespace W10_Logon_BG_Changer___Command_Line
{
    internal class Program
    {
        private readonly static string _newPriLocation = Path.Combine(Path.GetTempPath(), "new_temp_pri.pri");
        private readonly static string _tempPriFile = Path.Combine(Path.GetTempPath(), "bak_temp_pri.pri");

        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var filedir = args[0];
                if (File.Exists(filedir) && (Path.GetExtension(filedir) == ".jpg"))
                {
                    ChangeImage();
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("File \"" + filedir + "\" does not exist, or the file is not an image file!");
                }
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Please format your command as shown: commandthing [imagedir]");
                Console.WriteLine("");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        private static void ChangeImage()
        {
            HelperLib.TakeOwnership(Config.LogonFolder);
            HelperLib.TakeOwnership(Config.PriFileLocation);

            if (!File.Exists(Config.BakPriFileLocation))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Warning]: Could not find Windows.UI.Logon.pri.bak file. Creating new.");
                Console.ForegroundColor = ConsoleColor.White;
                File.Copy(Config.PriFileLocation, Config.BakPriFileLocation);
            }

            HelperLib.TakeOwnership(Config.BakPriFileLocation);

            File.Copy(Config.BakPriFileLocation, _tempPriFile, true);

            if (File.Exists(Config.CurrentImageLocation))
            {
                var temp = Path.GetTempFileName();

                File.Copy(Config.CurrentImageLocation, temp, true);
            }
            if (File.Exists(_newPriLocation))
            {
                File.Delete(_newPriLocation);
            }

            File.Copy(filedir, Config.CurrentImageLocation, true);

            File.Copy(Config.BakPriFileLocation, _tempPriFile, true);

            var imagetemp = Path.GetTempFileName();

            PriBuilder.CreatePri(_tempPriFile, _newPriLocation, imagetemp);

            File.Copy(_newPriLocation, Config.PriFileLocation, true);
        }
    }
}