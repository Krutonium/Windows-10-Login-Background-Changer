using System;
using Microsoft.Win32;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace W10_Logon_BG_Changer.Tools
{
    internal class Helpers
    {
        public static bool IsBackgroundDisabled()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System"))
            {
                var o = key?.GetValue("DisableLogonBackgroundImage");
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
            var a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            var d = a < 0.5 ? 0 : 255;

            return Color.FromArgb(d, d, d);
        }

        [DllImport("shell32.dll", EntryPoint = "#261",
              CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void GetUserTilePath(
          string username,
          uint whatever, // 0x80000000
          StringBuilder picpath, int maxLength);

        public static string GetUserTilePath(string username)
        {   // username: use null for current user
            var sb = new StringBuilder(1000);
            GetUserTilePath(username, 0x80000000, sb, sb.Capacity);
            return sb.ToString();
        }

        public static Image GetUserTile(string username) => Image.FromFile(GetUserTilePath(username));
    }
}