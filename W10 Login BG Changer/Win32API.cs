using System.Runtime.InteropServices;

namespace W10_Login_BG_Changer
{
    public static class Win32Api
    {
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();
    }
}