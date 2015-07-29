using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.Security.Win32Structs;

namespace Microsoft.Win32.Security
{
    /// <summary>
    /// Summary description for TokenPrivileges.
    /// </summary>
    public class TokenPrivileges : CollectionBase
    {
        public TokenPrivilege this[int index]
        {
            get { return (TokenPrivilege) base.InnerList[index]; }
        }

        public void Add(TokenPrivilege privilege)
        {
            base.InnerList.Add(privilege);
        }

        public unsafe byte[] GetNativeTokenPrivileges()
        {
            Debug.Assert(Marshal.SizeOf(typeof (TOKEN_PRIVILEGES)) == 4);

            TOKEN_PRIVILEGES tp;
            tp.PrivilegeCount = (uint) Count;

            int cbLength =
                Marshal.SizeOf(typeof (TOKEN_PRIVILEGES)) +
                Marshal.SizeOf(typeof (LUID_AND_ATTRIBUTES))*Count;
            var res = new byte[cbLength];
            fixed (byte* privs = res)
            {
                Marshal.StructureToPtr(tp, (IntPtr) privs, false);
            }

            int resOffset = Marshal.SizeOf(typeof (TOKEN_PRIVILEGES));
            for (int i = 0; i < Count; i++)
            {
                byte[] luida = this[i].GetNativeLUID_AND_ATTRIBUTES();
                Array.Copy(luida, 0, res, resOffset, luida.Length);
                resOffset += luida.Length;
            }
            return res;
        }
    }
}