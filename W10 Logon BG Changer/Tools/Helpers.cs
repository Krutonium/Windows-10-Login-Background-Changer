using Microsoft.Win32;

namespace W10_Logon_BG_Changer.Tools
{
    class Helpers
    {
        public static bool IsBackgroundDisabled()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System"))
            {
                if (key == null) return false;
                var o = key.GetValue("DisableLogonBackgroundImage");
                if (o != null)
                {
                    return o.ToString() == "1" || (int) o == 1;
                }
            }

            return false;
        }
    }
}
