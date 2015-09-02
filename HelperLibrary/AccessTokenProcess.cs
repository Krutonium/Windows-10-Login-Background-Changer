using System;

namespace HelperLibrary
{
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
                return rc == Win32.FALSE ? IntPtr.Zero : handle;
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