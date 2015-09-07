using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HelperLibrary
{
    /// <summary>
    ///     Summary description for TokenPrivileges.
    /// </summary>
    public class TokenPrivileges : CollectionBase
    {
        public TokenPrivilege this[int index] => (TokenPrivilege)InnerList[index];

        public void Add(TokenPrivilege privilege) => InnerList.Add(privilege);

        public unsafe byte[] GetNativeTokenPrivileges()
        {
            Debug.Assert(Marshal.SizeOf(typeof(TOKEN_PRIVILEGES)) == 4);

            TOKEN_PRIVILEGES tp;
            tp.PrivilegeCount = (uint)Count;

            var cbLength =
                Marshal.SizeOf(typeof(TOKEN_PRIVILEGES)) +
                Marshal.SizeOf(typeof(LUID_AND_ATTRIBUTES)) * Count;
            var res = new byte[cbLength];
            fixed (byte* privs = res)
            {
                Marshal.StructureToPtr(tp, (IntPtr)privs, false);
            }

            var resOffset = Marshal.SizeOf(typeof(TOKEN_PRIVILEGES));
            for (var i = 0; i < Count; i++)
            {
                var luida = this[i].GetNativeLUID_AND_ATTRIBUTES();
                Array.Copy(luida, 0, res, resOffset, luida.Length);
                resOffset += luida.Length;
            }
            return res;
        }
    }
}