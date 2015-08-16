using HelperLibrary;
using SharedLibrary;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace W10_Logon_BG_Changer___Command_Line
{
    internal class Program
    {
        private static readonly string NewPriLocation = Path.Combine(Path.GetTempPath(), "new_temp_pri.pri");
        private static readonly string TempPriFile = Path.Combine(Path.GetTempPath(), "bak_temp_pri.pri");

        private static void Main(string[] args)
        {
            TakeOwnership();

            foreach (var arg in args)
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
                        return;

                    case "/C":
                        var hex = arg.Substring(2);
                        try
                        {
                            if (hex.IsHex())
                            {
                                var converter = new ColorConverter();
                                var convertFromString = converter.ConvertFromString(hex);
                                if (convertFromString != null)
                                {
                                    var color = (Color)convertFromString;

                                    ChangeColor(color);
                                }
                                else
                                {
                                    return;
                                }

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("");
                                Console.WriteLine("Success!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("");
                                Console.WriteLine(@"""{0}"" is not a valid hex color code!", hex);
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
                        return;

                    case "/H":
                        Console.WriteLine("");
                        Console.WriteLine("HELP");
                        Console.WriteLine("=====");
                        Console.WriteLine("");
                        Console.WriteLine("AUTHORS - /A");
                        Console.WriteLine("COLOR - /C[HEX CODE]");
                        Console.WriteLine("IMAGE - /I[IMAGE PATH]");
                        Console.WriteLine("RESTORE - /R");
                        Console.WriteLine("HELP - /H");
                        return;

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
                        return;

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
                        return;

                    default:
                        Console.WriteLine("");
                        Console.WriteLine(
                            @"Please format your command as shown: exe [/A|/C|/H|/I|/R] [/CHEX CODE|/IFILE PATH]");
                        Console.WriteLine("");
                        Console.WriteLine(@"Press any key to exit.");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private static void ChangeImage(string filedir)
        {
            File.Copy(Config.BakPriFileLocation, TempPriFile, true);

            LogonPriEditor.ModifyLogonPri(TempPriFile, NewPriLocation, filedir);

            File.Copy(NewPriLocation, Config.PriFileLocation, true);
        }

        private static void Restore()
        {
            File.Copy(Config.BakPriFileLocation, Config.PriFileLocation, true);
        }

        private static string FillImageColor(Color c)
        {
            var image = Path.GetTempFileName();

            DrawFilledRectangle(3840, 2160, new SolidBrush(c)).Save(image, ImageFormat.Jpeg);

            return image;
        }

        private static Bitmap DrawFilledRectangle(int x, int y, Brush b)
        {
            var bmp = new Bitmap(x, y);
            using (var graph = Graphics.FromImage(bmp))
            {
                var imageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(b, imageSize);
            }
            return bmp;
        }

        private static void ChangeColor(Color c)
        {
            var image = FillImageColor(c);

            File.Copy(Config.BakPriFileLocation, TempPriFile, true);

            LogonPriEditor.ModifyLogonPri(TempPriFile, NewPriLocation, image);

            File.Copy(NewPriLocation, Config.PriFileLocation, true);
        }

        private static void TakeOwnership()
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
        }
    }
}