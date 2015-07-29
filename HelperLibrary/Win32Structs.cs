using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.Security.Win32Structs
{
    using HANDLE = IntPtr;
    using DWORD = UInt32;
    using LONG = Int32;
    using UCHAR = Byte;
    using BOOL = Int32;
    using LARGE_INTEGER = Int64;
    using PACL = IntPtr;
    using PSID = IntPtr;
    using GUID = Guid;
    using PVOID = IntPtr;
    using LPWSTR = String;

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