using System;

namespace Microsoft.Win32.Security
{
    [Flags]
    public enum AccessType : uint
    {
        DELETE = 0x00010000,
        READ_CONTROL = 0x00020000,
        WRITE_DAC = 0x00040000,
        WRITE_OWNER = 0x00080000,
        SYNCHRONIZE = 0x00100000,

        STANDARD_RIGHTS_REQUIRED = 0x000F0000,

        STANDARD_RIGHTS_READ = READ_CONTROL,
        STANDARD_RIGHTS_WRITE = READ_CONTROL,
        STANDARD_RIGHTS_EXECUTE = READ_CONTROL,

        STANDARD_RIGHTS_ALL = 0x001F0000,

        SPECIFIC_RIGHTS_ALL = 0x0000FFFF,

        //
        // AccessSystemAcl access type
        //

        ACCESS_SYSTEM_SECURITY = 0x01000000,

        //
        // MaximumAllowed access type
        //

        MAXIMUM_ALLOWED = 0x02000000,

        //
        //  These are the generic rights.
        //

        GENERIC_READ = 0x80000000,
        GENERIC_WRITE = 0x40000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_ALL = 0x10000000,
    }

    [Flags]
    public enum TokenAccessType : uint
    {
        TOKEN_ASSIGN_PRIMARY = 0x0001,
        TOKEN_DUPLICATE = 0x0002,
        TOKEN_IMPERSONATE = 0x0004,
        TOKEN_QUERY = 0x0008,
        TOKEN_QUERY_SOURCE = 0x0010,
        TOKEN_ADJUST_PRIVILEGES = 0x0020,
        TOKEN_ADJUST_GROUPS = 0x0040,
        TOKEN_ADJUST_DEFAULT = 0x0080,
        TOKEN_ADJUST_SESSIONID = 0x0100,

        TOKEN_ALL_ACCESS =
            AccessType.STANDARD_RIGHTS_REQUIRED |
            TOKEN_ASSIGN_PRIMARY |
            TOKEN_DUPLICATE |
            TOKEN_IMPERSONATE |
            TOKEN_QUERY |
            TOKEN_QUERY_SOURCE |
            TOKEN_ADJUST_PRIVILEGES |
            TOKEN_ADJUST_GROUPS |
            TOKEN_ADJUST_DEFAULT |
            TOKEN_ADJUST_SESSIONID,

        TOKEN_READ =
            AccessType.STANDARD_RIGHTS_READ |
            TOKEN_QUERY,

        TOKEN_WRITE =
            AccessType.STANDARD_RIGHTS_WRITE |
            TOKEN_ADJUST_PRIVILEGES |
            TOKEN_ADJUST_GROUPS |
            TOKEN_ADJUST_DEFAULT,

        TOKEN_EXECUTE =
            AccessType.STANDARD_RIGHTS_EXECUTE,
    }

    [Flags]
    public enum ProcessAccessType : uint
    {
        DELETE = 0x00010000,
        READ_CONTROL = 0x00020000,
        WRITE_DAC = 0x00040000,
        WRITE_OWNER = 0x00080000,
        SYNCHRONIZE = 0x00100000,

        STANDARD_RIGHTS_REQUIRED = 0x000F0000,

        STANDARD_RIGHTS_READ = READ_CONTROL,
        STANDARD_RIGHTS_WRITE = READ_CONTROL,
        STANDARD_RIGHTS_EXECUTE = READ_CONTROL,

        STANDARD_RIGHTS_ALL = 0x001F0000,

        SPECIFIC_RIGHTS_ALL = 0x0000FFFF,

        //
        // AccessSystemAcl access type
        //

        ACCESS_SYSTEM_SECURITY = 0x01000000,

        //
        // MaximumAllowed access type
        //

        MAXIMUM_ALLOWED = 0x02000000,

        //
        //  These are the generic rights.
        //

        GENERIC_READ = 0x80000000,
        GENERIC_WRITE = 0x40000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_ALL = 0x10000000,

        // PROCESS specific
        PROCESS_TERMINATE = 0x0001,
        PROCESS_CREATE_THREAD = 0x0002,
        PROCESS_SET_SESSIONID = 0x0004,
        PROCESS_VM_OPERATION = 0x0008,
        PROCESS_VM_READ = 0x0010,
        PROCESS_VM_WRITE = 0x0020,
        PROCESS_DUP_HANDLE = 0x0040,
        PROCESS_CREATE_PROCESS = 0x0080,
        PROCESS_SET_QUOTA = 0x0100,
        PROCESS_SET_INFORMATION = 0x0200,
        PROCESS_QUERY_INFORMATION = 0x0400,
        PROCESS_SUSPEND_RESUME = 0x0800,

        PROCESS_ALL_ACCESS =
            AccessType.STANDARD_RIGHTS_REQUIRED |
            AccessType.SYNCHRONIZE |
            0xFFF,
    }

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
        UsedForAccess = 0x80000000,
    }


    [Flags]
    public enum SecurityDescriptorControlFlags : ushort // WORD
    {
        SE_OWNER_DEFAULTED = 0x0001,
        SE_GROUP_DEFAULTED = 0x0002,
        SE_DACL_PRESENT = 0x0004,
        SE_DACL_DEFAULTED = 0x0008,
        SE_SACL_PRESENT = 0x0010,
        SE_SACL_DEFAULTED = 0x0020,
        SE_DACL_AUTO_INHERIT_REQ = 0x0100,
        SE_SACL_AUTO_INHERIT_REQ = 0x0200,
        SE_DACL_AUTO_INHERITED = 0x0400,
        SE_SACL_AUTO_INHERITED = 0x0800,
        SE_DACL_PROTECTED = 0x1000,
        SE_SACL_PROTECTED = 0x2000,
        SE_RM_CONTROL_VALID = 0x4000,
        SE_SELF_RELATIVE = 0x8000,
    }
}