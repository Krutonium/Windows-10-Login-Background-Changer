using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace W10_BG_Logon_Changer.Tools.Animations
{
    public static class ImageFader
    {
        private const int Speed = 1;
        private const double Fadespeed = 0.10;

        public async static void Fadein(Image image)
        {
            for (double i = 0; i < 1.01; i += Fadespeed)
            {
                i = Math.Round(i, 2);

                await Task.Delay(Speed);

                image.Opacity = i;
            }
        }

        public async static void Fadeout(Image image)
        {
            for (double i = 1; i > -0.01; i -= Fadespeed)
            {
                i = Math.Round(i, 2);

                await Task.Delay(Speed);

                image.Opacity = i;
            }
        }
    }
}
