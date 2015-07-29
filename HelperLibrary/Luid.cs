using Microsoft.Win32.Security.Win32Structs;

namespace Microsoft.Win32.Security
{
    /// <summary>
    /// Summary description for Luid.
    /// </summary>
    public class Luid
    {
        private readonly LUID _luid;

        public Luid(LUID luid)
        {
            _luid = luid;
        }

        internal LUID GetNativeLUID()
        {
            return _luid;
        }
    }
}