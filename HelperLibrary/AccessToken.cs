using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.Security
{
    /// <summary>
    ///     Encapsulation of a Win32 token handle.
    ///     The object is disposable because it maintains the Win32 handle.
    /// </summary>
    public abstract class AccessToken : DisposableObject
    {
        /// The win32 token handle.
        private IntPtr _handle;

        protected internal AccessToken(IntPtr handle)
        {
            _handle = handle;
        }

        protected override void Dispose(bool disposing)
        {
            if (_handle != IntPtr.Zero)
            {
                // We don't want to throw an exception here, because there's not
                // much we can do when failing to close a handle.
                var rc = Win32.CloseHandle(_handle);
                if (rc != Win32.FALSE)
                    _handle = IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Enable a single privilege on the process.
        /// </summary>
        /// <param name="privilege"></param>
        /// <exception cref="">
        ///     Throws an exception if the privilege is not present
        ///     in the privilege list of the process
        /// </exception>
        public void EnablePrivilege(TokenPrivilege privilege)
        {
            var privs = new TokenPrivileges { privilege };
            EnableDisablePrivileges(privs);
        }

        private void EnableDisablePrivileges(TokenPrivileges privileges)
        {
            UnsafeEnableDisablePrivileges(privileges);
        }

        private unsafe void UnsafeEnableDisablePrivileges(TokenPrivileges privileges)
        {
            var privBytes = privileges.GetNativeTokenPrivileges();
            fixed (byte* priv = privBytes)
            {
                uint cbLength;

                Win32.SetLastError(Win32.SUCCESS);

                var rc = Win32.AdjustTokenPrivileges(
                    _handle,
                    Win32.FALSE,
                    (IntPtr)priv,
                    0,
                    IntPtr.Zero,
                    out cbLength);
                Win32.CheckCall(rc);

                // Additional check: privilege can't be added, and in that case,
                // rc indicates a success, but GetLastError() has a specific meaning.
                if (Marshal.GetLastWin32Error() == Win32.ERROR_NOT_ALL_ASSIGNED)
                    Win32.ThrowLastError();
            }
        }
    }

    /// <summary>
    ///     Access token for a process
    /// </summary>
    public class AccessTokenProcess : AccessToken
    {
        public AccessTokenProcess(int pid, TokenAccessType desiredAccess)
            : base(OpenProcessToken(pid, desiredAccess))
        {
        }

        private static IntPtr TryOpenProcessToken(int pid, TokenAccessType desiredAccess)
        {
            var processHandle = Win32.OpenProcess(
                ProcessAccessType.PROCESS_QUERY_INFORMATION,
                Win32.FALSE,
                (uint)pid);
            if (processHandle == IntPtr.Zero)
                return IntPtr.Zero;
            Win32.CheckCall(processHandle);
            try
            {
                IntPtr handle;
                var rc = Win32.OpenProcessToken(processHandle, desiredAccess, out handle);
                if (rc == Win32.FALSE)
                    return IntPtr.Zero;
                return handle;
            }
            finally
            {
                Win32.CloseHandle(processHandle);
            }
        }

        private static IntPtr OpenProcessToken(int pid, TokenAccessType desiredAccess)
        {
            var handle = TryOpenProcessToken(pid, desiredAccess);
            if (handle == IntPtr.Zero)
                Win32.ThrowLastError();
            return handle;
        }
    }
}