using System.Runtime.InteropServices;

namespace HelperLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TOKEN_PRIVILEGES
    {
        public uint PrivilegeCount;
        // Followed by this:
        //LUID_AND_ATTRIBUTES Privileges[ANYSIZE_ARRAY];
    }
}