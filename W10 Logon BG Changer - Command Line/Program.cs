using HelperLibrary;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        public static bool TestIfHex(string hex)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(hex, @"\A\b[0-9a-fA-F]+\b\Z");
            //return Int32.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }

        private static void Main(string[] args)
        {
            switch (args[0])
            {
                case null:
                    Console.WriteLine("");
                    Console.WriteLine(@"Please format your command as shown: commandthing [color|image] [hex code|image path]");
                    Console.WriteLine("");
                    Console.WriteLine(@"Press any key to exit.");
                    Console.ReadKey();
                    return;

                case "authors":
                    Console.WriteLine("");
                    Console.WriteLine(@"Syrexide | https://github.com/Syrexide/");
                    Console.WriteLine(@"Toyz | https://github.com/Toyz/");
                    return;

                case "color":
                    var hex = args[3];
                    var isHex = TestIfHex(hex);
                    if (isHex == true)
                    {
                        var converter = new System.Drawing.ColorConverter();
                        Color color = (Color)converter.ConvertFromString(hex);

                        ChangeColor(color);
                    }
                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine(@"""{0}"" is not a valid hex color code!", hex);
                    }

                    return;

                case "image":
                    var filedir = args[0];
                    if (File.Exists(filedir) && (Path.GetExtension(filedir) == ".jpg") ||
                        (Path.GetExtension(filedir) == ".png") || (Path.GetExtension(filedir) == ".bmp") ||
                        (Path.GetExtension(filedir) == ".tif") || (Path.GetExtension(filedir) == ".tiff"))
                    {
                        ChangeImage(filedir);
                    }
                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine(@"File ""{0}"" does not exist, or the file is not an image file!", filedir);
                    }
                    return;

                case "restore":
                    Restore();
                    return;
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

        private static string ChangeColor(Color color)
        {
            var image = Path.GetTempFileName();

            DrawFilledRectangle(3840, 2160, new SolidBrush(color)).Save(image, ImageFormat.Jpeg);

            return image;
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