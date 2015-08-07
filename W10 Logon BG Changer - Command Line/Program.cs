using HelperLibrary;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using W10_Logon_BG_Changer___Command_Line.Tools;
using Color = System.Drawing.Color;
using Path = System.IO.Path;
using Rectangle = System.Drawing.Rectangle;

namespace W10_Logon_BG_Changer___Command_Line
{
    internal class Program
    {
        private readonly static string NewPriLocation = Path.Combine(Path.GetTempPath(), "new_temp_pri.pri");
        private readonly static string TempPriFile = Path.Combine(Path.GetTempPath(), "bak_temp_pri.pri");

        private static bool TestIfHex(string hex)
        {
            return (Regex.Match(hex, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success);
        }

        private static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                switch (arg.Substring(0, 2).ToUpper())
                {
                    case "/A":
                        try
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("");
                            Console.WriteLine(@"Syrexide | https://github.com/Syrexide/");
                            Console.WriteLine(@"Toyz | https://github.com/Toyz/");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("");
                            Console.WriteLine(@"An error occurred: '{0}'", e);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;

                    case "/C":
                        var hex = arg.Substring(2);
                        try
                        {
                            var isHex = TestIfHex(hex);
                            if (isHex)
                            {
                                var converter = new ColorConverter();
                                Color color = (Color)converter.ConvertFromString(hex);

                                Program p = new Program();
                                p.ChangeColor(color);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("");
                                Console.WriteLine("Success!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.WriteLine("");
                                Console.WriteLine(@"""{0}"" is not a valid hex color code!", hex);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("");
                            Console.WriteLine(@"An error occurred: '{0}'", e);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;

                    case "/H":
                        Console.WriteLine("");
                        Console.WriteLine("HELP");
                        Console.WriteLine("=====");
                        Console.WriteLine("");
                        Console.WriteLine("AUTHORS - /A");
                        Console.WriteLine("COLOR - /C [HEX CODE]");
                        Console.WriteLine("IMAGE - /I [IMAGE PATH]");
                        Console.WriteLine("RESTORE - /R");
                        Console.WriteLine("HELP - /H");
                        break;

                    case "/I":
                        var filedir = arg.Substring(2);
                        try
                        {
                            if (File.Exists(filedir) && (Path.GetExtension(filedir) == ".jpg") ||
                                (Path.GetExtension(filedir) == ".png") || (Path.GetExtension(filedir) == ".bmp") ||
                                (Path.GetExtension(filedir) == ".tif") || (Path.GetExtension(filedir) == ".tiff"))
                            {
                                ChangeImage(filedir);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("");
                                Console.WriteLine("Success!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("");
                                Console.WriteLine("An error occurred: The file \"" + filedir + "\" is not a valid file!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("");
                            Console.WriteLine(@"An error occurred: '{0}'", e);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;

                    case "/R":
                        try
                        {
                            Restore();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("");
                            Console.WriteLine("Success!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("");
                            Console.WriteLine(@"An error occurred: '{0}'", e);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;

                    default:
                        Console.WriteLine("");
                        Console.WriteLine(@"Please format your command as shown: exe [/A|/C|/H|/I|/R] [/CHEX CODE|/IFILE PATH]");
                        Console.WriteLine("");
                        Console.WriteLine(@"Press any key to exit.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ChangeImage(string filedir)
        {
            HelperLib.TakeOwnership(Config.LogonFolder);

            HelperLib.TakeOwnership(Config.PriFileLocation);

            if (!File.Exists(Config.BakPriFileLocation))
            {
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

        private string ChangeColor(Color c)
        {
            var image = Path.GetTempFileName();

            DrawFilledRectangle(3840, 2160, new SolidBrush(c)).Save(image, ImageFormat.Jpeg);

            return image;

            // below is unreachable code

            HelperLib.TakeOwnership(Config.LogonFolder);

            HelperLib.TakeOwnership(Config.PriFileLocation);

            if (!File.Exists(Config.BakPriFileLocation))
            {
                File.Copy(Config.PriFileLocation, Config.BakPriFileLocation);
            }

            HelperLib.TakeOwnership(Config.BakPriFileLocation);

            File.Copy(Config.BakPriFileLocation, TempPriFile, true);

            if (File.Exists(NewPriLocation))
            {
                File.Delete(NewPriLocation);
            }

            File.Copy(Config.BakPriFileLocation, TempPriFile, true);

            PriBuilder.CreatePri(TempPriFile, NewPriLocation, image);

            File.Copy(NewPriLocation, Config.PriFileLocation, true);
        }

        private static Bitmap DrawFilledRectangle(int x, int y, System.Drawing.Brush b)
        {
            var bmp = new Bitmap(x, y);
            using (var graph = Graphics.FromImage(bmp))
            {
                var imageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(b, imageSize);
            }
            return bmp;
        }
    }
}