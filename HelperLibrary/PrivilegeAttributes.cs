using System;

namespace HelperLibrary
{
    [Flags]
    public enum PrivilegeAttributes : uint
    {
        /*
        SE_PRIVILEGE_DISABLED            = 0,
        SE_PRIVILEGE_ENABLED_BY_DEFAULT  = 0x00000001,
        SE_PRIVILEGE_ENABLED             = 0x00000002,
        SE_PRIVILEGE_USED_FOR_ACCESS     = 0x80000000,
        */
        Disabled = 0,
        EnabledByDefault = 0x00000001,
        Enabled = 0x00000002,
        UsedForAccess = 0x80000000
    }
}