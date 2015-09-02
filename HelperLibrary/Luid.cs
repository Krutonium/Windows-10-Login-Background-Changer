namespace HelperLibrary
{
    /// <summary>
    ///     Summary description for Luid.
    /// </summary>
    public class Luid
    {
        private readonly LUID _luid;

        public Luid(LUID luid)
        {
            _luid = luid;
        }

        internal LUID GetNativeLuid() => _luid;
    }
}