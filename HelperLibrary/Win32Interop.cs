using Microsoft.Win32.Security.Win32Structs;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.Security
{
    using BOOL = Int32;
    using DWORD = UInt32;
    using HANDLE = IntPtr;

    using PTOKEN_PRIVILEGES = IntPtr;

    /// <summary>
    /// Summary description for Win32Interop.
    /// </summary>
    public class Win32
    {
        public const BOOL FALSE = 0;
        public const BOOL TRUE = 1;

        public const int SUCCESS = 0;
        public const int ERROR_SUCCESS = 0;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int ERROR_NOT_ALL_ASSIGNED = 1300;
        public const int ERROR_NONE_MAPPED = 1332;
        private const string Kernel32 = "kernel32.dll";
        private const string Advapi32 = "Advapi32.dll";

        public static DWORD GetLastError()
        {
            return (DWORD)Marshal.GetLastWin32Error();
        }

        public static void ThrowLastError()
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        public static void CheckCall(bool funcResult)
        {
            if (!funcResult)
            {
                ThrowLastError();
            }
        }

        public static void CheckCall(BOOL funcResult)
        {
            CheckCall(funcResult != 0);
        }

        public static void CheckCall(HANDLE funcResult)
        {
            CheckCall(!IsNullHandle(funcResult));
        }

        public static bool IsNullHandle(HANDLE ptr)
        {
            return (ptr == IntPtr.Zero);
        }

        ///////////////////////////////////////////////////////////////////////////////
        ///
        /// KERNEL32.DLL
        ///
        [DllImport(Kernel32, CallingConvention = CallingConvention.Winapi)]
        public static extern void SetLastError(DWORD dwErrCode);

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
        public static extern HANDLE OpenProcess(ProcessAccessType dwDesiredAccess, BOOL bInheritHandle,
                                                DWORD dwProcessId);

        [DllImport(Kernel32, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern BOOL CloseHandle(HANDLE handle);

        ///////////////////////////////////////////////////////////////////////////////
        ///
        /// ADVAPI32.DLL
        ///
        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern BOOL OpenProcessToken(HANDLE hProcess, TokenAccessType dwDesiredAccess, out HANDLE hToken);

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern BOOL LookupPrivilegeValue(string lpSystemName, string lpName, out LUID Luid);

        [DllImport(Advapi32, CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern BOOL AdjustTokenPrivileges(
            HANDLE TokenHandle,
            BOOL DisableAllPrivileges,
            PTOKEN_PRIVILEGES NewState,
            DWORD BufferLength,
            PTOKEN_PRIVILEGES PreviousState,
            out DWORD ReturnLength);
    }
}