using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using HelperLibrary;
using SharedLibrary;

namespace W10_Logon_BG_Changer___Command_Line.Helpers
{
    internal class ImageHelper
    {
        static readonly string NewPriLocation = Path.Combine(Path.GetTempPath(), "new_temp_pri.pri");
        static readonly string TempPriFile = Path.Combine(Path.GetTempPath(), "bak_temp_pri.pri");

        public static void ChangeImage(string filedir)
        {
            File.Copy(Config.BakPriFileLocation, TempPriFile, true);
            LogonPriEditor.ModifyLogonPri(TempPriFile, NewPriLocation, filedir);
            File.Copy(NewPriLocation, Config.PriFileLocation, true);
        }

        public static void Restore() => File.Copy(Config.BakPriFileLocation, Config.PriFileLocation, true);

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

        public static void ChangeColor(string hexColorCode)
        {
            var convertFromString = new ColorConverter().ConvertFromString(hexColorCode);
            if (convertFromString != null)
            {
                var color = (Color)convertFromString;
                var imagePath = Path.GetTempFileName();
                DrawFilledRectangle(3840, 2160, new SolidBrush(color)).Save(imagePath, ImageFormat.Jpeg);
                File.Copy(Config.BakPriFileLocation, TempPriFile, true);
                LogonPriEditor.ModifyLogonPri(TempPriFile, NewPriLocation, imagePath);
            }
            File.Copy(NewPriLocation, Config.PriFileLocation, true);
        }

        public static void TakeOwnership()
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
