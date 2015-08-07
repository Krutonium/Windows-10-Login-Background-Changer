using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace W10_Logon_BG_Changer.Tools.Animations
{
    public static class ImageFader
    {
        private const int interval = 1;
        private const double speed = 0.10;

        public static async void fadeIn(Image image)
        {
            for (double i = 0; i < 1.01; i += speed)
            {
                i = Math.Round(i, 2);

                await Task.Delay(interval);

                Debug.WriteLine("[" + image.Name + "] fadeIn: " + i);

                image.Opacity = i;
            }
        }

        public static async void fadeOut(Image image)
        {
            for (double i = 1; i > -0.01; i -= speed)
            {
                i = Math.Round(i, 2);

                await Task.Delay(interval);

                Debug.WriteLine("[" + image.Name + "] fadeOut: " + i);

                image.Opacity = i;
            }
        }
    }
}