using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace W10_Logon_BG_Changer.Tools.Animations
{
    public static class ControlFader
    {
        private const int Interval = 1;
        private const double Speed = 0.10;

        public static async void FadeIn(FrameworkElement control)
        {
            for (double i = 0; i < 1.01; i += Speed)
            {
                i = Math.Round(i, 2);

                await Task.Delay(Interval);

                Debug.WriteLine($"[{control.Name}] fadeIn: {i}");

                control.Opacity = i;
            }
        }

        public static async void FadeOut(FrameworkElement control)
        {
            for (double i = 1; i > -0.01; i -= Speed)
            {
                i = Math.Round(i, 2);

                await Task.Delay(Interval);

                Debug.WriteLine($"[{control.Name}] fadeOut: {i}");

                control.Opacity = i;
            }
        }
    }
}