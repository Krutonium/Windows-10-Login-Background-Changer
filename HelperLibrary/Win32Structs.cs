using System.Runtime.InteropServices;

namespace Microsoft.Win32.Security.Win32Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public uint LowPart;
        public int HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TOKEN_PRIVILEGES
    {
        public uint PrivilegeCount;
        // Followed by this:
        //LUID_AND_ATTRIBUTES Privileges[ANYSIZE_ARRAY];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public uint Attributes;
    }
}