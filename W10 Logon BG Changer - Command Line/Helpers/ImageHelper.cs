using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using HelperLibrary;
using SharedLibrary;

namespace W10_Logon_BG_Changer___Command_Line.Helpers
{
    internal class ImageHelper
    {
        readonly string NewPriLocation = Path.Combine(Path.GetTempPath(), "new_temp_pri.pri");
        readonly string TempPriFile = Path.Combine(Path.GetTempPath(), "bak_temp_pri.pri");

        public void ChangeImage(string filedir)
        {
            File.Copy(Config.BakPriFileLocation, TempPriFile, true);
            LogonPriEditor.ModifyLogonPri(TempPriFile, NewPriLocation, filedir);
            File.Copy(NewPriLocation, Config.PriFileLocation, true);
        }

        public void Restore()
        {
            File.Copy(Config.BakPriFileLocation, Config.PriFileLocation, true);
        }

        private Bitmap DrawFilledRectangle(int x, int y, Brush b)
        {
            var bmp = new Bitmap(x, y);

            using (var graph = Graphics.FromImage(bmp))
            {
                var imageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(b, imageSize);
            }

            return bmp;
        }

        public void ChangeColor(string hexColorCode)
        {
            Color color = (Color)new ColorConverter().ConvertFromString(hexColorCode);
            string imagePath = Path.GetTempFileName();
            DrawFilledRectangle(3840, 2160, new SolidBrush(color)).Save(imagePath, ImageFormat.Jpeg);
            File.Copy(Config.BakPriFileLocation, TempPriFile, true);
            LogonPriEditor.ModifyLogonPri(TempPriFile, NewPriLocation, imagePath);
            File.Copy(NewPriLocation, Config.PriFileLocation, true);
        }

        public void TakeOwnership()
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
