using Microsoft.Win32;
using System.Drawing;

namespace W10_Logon_BG_Changer.Tools
{
    internal class Helpers
    {
        public static bool IsBackgroundDisabled()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System"))
            {
                if (key == null) return false;
                var o = key.GetValue("DisableLogonBackgroundImage");
                if (o != null)
                {
                    return o.ToString() == "1" || (int)o == 1;
                }
            }

            return false;
        }

        public static Color ContrastColor(Color color)
        {
            // Counting the perceptive luminance - human eye favors green color...
            double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            var d = a < 0.5 ? 0 : 255;

            return Color.FromArgb(d, d, d);
        }
    }
}