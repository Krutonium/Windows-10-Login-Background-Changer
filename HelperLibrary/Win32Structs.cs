using System.Runtime.InteropServices;

namespace HelperLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public uint LowPart;
        public int HighPart;
    }
}