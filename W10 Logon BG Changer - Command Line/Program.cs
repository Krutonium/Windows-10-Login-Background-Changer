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


            if (args.Length <= 0)
            {
                Console.WriteLine("Please use '/h' for help");
                return;
            }

            if (!args[0].ToLower().StartsWith("/") || !args[0].ToLower().StartsWith("-"))
            {
                Console.WriteLine("Please use '/h' for help");
                return;
            }

            switch (args[0].ToLower().Replace("/", "").Replace("-", ""))
            {
                case "h":
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

                case "a":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("");
                    Console.WriteLine(@"Syrexide | https://github.com/Syrexide/");
                    Console.WriteLine(@"Toyz | https://github.com/Toyz/");
                    Console.ResetColor();
                    break;

                case "c":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: /c hexcode (Example /c 000000");
                        return;
                    }

                    var hex = args[1];

                    if (!hex.IsHex())
                    {
                        Console.WriteLine("Not a valid Hex color!");
                        return;
                    }

                    var converter = new ColorConverter();
                    var convertFromString = converter.ConvertFromString(hex);

                    if(convertFromString != null)
                        ChangeColor((Color)convertFromString);
                    break;

                case "i":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: /i pathtoimage");
                        return;
                    }

                    var filedir = args[1];

                    if (File.Exists(filedir) && (Path.GetExtension(filedir) == ".jpg") ||
                                (Path.GetExtension(filedir) == ".png") || (Path.GetExtension(filedir) == ".bmp") ||
                                (Path.GetExtension(filedir) == ".tif") || (Path.GetExtension(filedir) == ".tiff"))
                    {
                        ChangeImage(filedir);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("");
                        Console.WriteLine("Success!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("");
                        Console.WriteLine("An error occurred: The file \"{0}\" is not a valid file!", filedir);
                        Console.ResetColor();
                    }
                    break;

                case "r":
                    try
                    {
                        Restore();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("");
                        Console.WriteLine("Success!");
                        Console.ResetColor();
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("");
                        Console.WriteLine(@"An error occurred: '{0}'", e);
                        Console.ResetColor();
                    }
                    return;

                default:
                    Console.WriteLine("Please use '/h' for help");
                    return;
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