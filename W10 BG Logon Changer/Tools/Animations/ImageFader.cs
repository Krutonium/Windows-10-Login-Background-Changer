using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace W10_BG_Logon_Changer.Tools.Animations
{
    public static class ImageFader
    {
        private static int _speed = 1;
        public async static void Fadein(Image image)
        {
            for (double i = 0; i < 1.01; i += 0.01)
            {
                i = Math.Round(i, 2);

                await Task.Delay(_speed);

                image.Opacity = i;
            }
        }

        public async static void Fadeout(Image image)
        {
            for (double i = 1; i > -0.01; i -= 0.01)
            {
                i = Math.Round(i, 2);

                await Task.Delay(_speed);

                image.Opacity = i;
            }
        }
    }
}
