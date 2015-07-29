using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.Security.Win32Structs;

namespace Microsoft.Win32.Security
{
    using HANDLE = IntPtr;
    using DWORD = UInt32;
    using BOOL = Int32;
    using LPVOID = IntPtr;
    using PSID = IntPtr;

    /// <summary>
    /// Summary description for TokenPrivilege.
    /// </summary>
    public class TokenPrivilege
    {
        public const string SE_CREATE_TOKEN_NAME = "SeCreateTokenPrivilege";
        public const string SE_ASSIGNPRIMARYTOKEN_NAME = "SeAssignPrimaryTokenPrivilege";
        public const string SE_LOCK_MEMORY_NAME = "SeLockMemoryPrivilege";
        public const string SE_INCREASE_QUOTA_NAME = "SeIncreaseQuotaPrivilege";
        public const string SE_UNSOLICITED_INPUT_NAME = "SeUnsolicitedInputPrivilege";
        public const string SE_MACHINE_ACCOUNT_NAME = "SeMachineAccountPrivilege";
        public const string SE_TCB_NAME = "SeTcbPrivilege";
        public const string SE_SECURITY_NAME = "SeSecurityPrivilege";
        public const string SE_TAKE_OWNERSHIP_NAME = "SeTakeOwnershipPrivilege";
        public const string SE_LOAD_DRIVER_NAME = "SeLoadDriverPrivilege";
        public const string SE_SYSTEM_PROFILE_NAME = "SeSystemProfilePrivilege";
        public const string SE_SYSTEMTIME_NAME = "SeSystemtimePrivilege";
        public const string SE_PROF_SINGLE_PROCESS_NAME = "SeProfileSingleProcessPrivilege";
        public const string SE_INC_BASE_PRIORITY_NAME = "SeIncreaseBasePriorityPrivilege";
        public const string SE_CREATE_PAGEFILE_NAME = "SeCreatePagefilePrivilege";
        public const string SE_CREATE_PERMANENT_NAME = "SeCreatePermanentPrivilege";
        public const string SE_BACKUP_NAME = "SeBackupPrivilege";
        public const string SE_RESTORE_NAME = "SeRestorePrivilege";
        public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        public const string SE_DEBUG_NAME = "SeDebugPrivilege";
        public const string SE_AUDIT_NAME = "SeAuditPrivilege";
        public const string SE_SYSTEM_ENVIRONMENT_NAME = "SeSystemEnvironmentPrivilege";
        public const string SE_CHANGE_NOTIFY_NAME = "SeChangeNotifyPrivilege";
        public const string SE_REMOTE_SHUTDOWN_NAME = "SeRemoteShutdownPrivilege";
        public const string SE_UNDOCK_NAME = "SeUndockPrivilege";
        public const string SE_SYNC_AGENT_NAME = "SeSyncAgentPrivilege";
        public const string SE_ENABLE_DELEGATION_NAME = "SeEnableDelegationPrivilege";
        public const string SE_MANAGE_VOLUME_NAME = "SeManageVolumePrivilege";

        private readonly PrivilegeAttributes _attributes;
        private readonly Luid _luid;

        public TokenPrivilege(string systemName, string privilege, bool enabled)
        {
            LUID luid;
            BOOL rc = Win32.LookupPrivilegeValue(systemName, privilege, out luid);
            Win32.CheckCall(rc);

            _luid = new Luid(luid);
            _attributes = (enabled ? PrivilegeAttributes.Enabled : 0);
        }

        public TokenPrivilege(string privilege, bool enabled) :
            this(null, privilege, enabled)
        {
        }

        public unsafe byte[] GetNativeLUID_AND_ATTRIBUTES()
        {
            LUID_AND_ATTRIBUTES la;
            la.Luid = _luid.GetNativeLUID();
            la.Attributes = (uint) _attributes;
            var res = new byte[Marshal.SizeOf(typeof (LUID_AND_ATTRIBUTES))];
            fixed (byte* luida = res)
            {
                Marshal.StructureToPtr(la, (IntPtr) luida, false);
            }
            return res;
        }
    }
}