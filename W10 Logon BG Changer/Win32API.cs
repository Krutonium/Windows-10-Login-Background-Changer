using System.Runtime.InteropServices;

namespace W10_BG_Logon_Changer
{
    public static class Win32Api
    {
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();
    }
}