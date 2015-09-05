using System;
using System.Runtime.InteropServices;

namespace HelperLibrary
{
    using HANDLE = IntPtr;

    using PTOKEN_PRIVILEGES = IntPtr;

    /// <summary>
    ///     Summary description for Win32Interop.
    /// </summary>
    public class Win32
    {
        public const int FALSE = 0;
        public const int TRUE = 1;
        public const int SUCCESS = 0;
        public const int ERROR_SUCCESS = 0;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int ERROR_NOT_ALL_ASSIGNED = 1300;
        public const int ERROR_NONE_MAPPED = 1332;
        private const string Kernel32 = "kernel32.dll";
        private const string Advapi32 = "Advapi32.dll";

        public static uint GetLastError() => (uint)Marshal.GetLastWin32Error();

        public static void ThrowLastError() => Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

        public static void CheckCall(bool funcResult)
        {
            if (!funcResult)
            {
                ThrowLastError();
            }
        }

        public static void CheckCall(int funcResult) => CheckCall(funcResult != 0);

        public static void CheckCall(HANDLE funcResult)
        {
            CheckCall(!IsNullHandle(funcResult));
        }

        public static bool IsNullHandle(HANDLE ptr) => (ptr == IntPtr.Zero);

        ///////////////////////////////////////////////////////////////////////////////
        /// KERNEL32.DLL
        [DllImport(Kernel32, CallingConvention = CallingConvention.Winapi)]
        public static extern void SetLastError(uint dwErrCode);

        /*
		[DllImport(Kernel32, CallingConvention=CallingConvention.Winapi, SetLastError=true, CharSet=CharSet.Auto)]
		public static extern HANDLE CreateEvent(
			[In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(SecurityAttributesMarshaler))]
			SecurityAttributes lpEventAttributes, // SA
			BOOL bManualReset,                    // reset type
			BOOL bInitialState,                   // initial state
			string lpName                         // object name
			);
		*/

        [DllImport(Kernel32, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern HANDLE OpenProcess(ProcessAccessType dwDesiredAccess, int bInheritHandle,
            uint dwProcessId);

        [DllImport(Kernel32, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern int CloseHandle(HANDLE handle);

        ///////////////////////////////////////////////////////////////////////////////
        /// ADVAPI32.DLL
        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern int OpenProcessToken(HANDLE hProcess, TokenAccessType dwDesiredAccess, out HANDLE hToken);

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int LookupPrivilegeValue(string lpSystemName, string lpName, out LUID Luid);

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int AdjustTokenPrivileges(
            HANDLE TokenHandle,
            int DisableAllPrivileges,
            PTOKEN_PRIVILEGES NewState,
            uint BufferLength,
            PTOKEN_PRIVILEGES PreviousState,
            out uint ReturnLength);
    }
}