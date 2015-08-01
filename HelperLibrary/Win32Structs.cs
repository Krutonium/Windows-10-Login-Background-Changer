using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.Security.Win32Structs
{
    using DWORD = UInt32;
    using LONG = Int32;

    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public DWORD LowPart;
        public LONG HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TOKEN_PRIVILEGES
    {
        public DWORD PrivilegeCount;
        // Followed by this:
        //LUID_AND_ATTRIBUTES Privileges[ANYSIZE_ARRAY];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public DWORD Attributes;
    }
}