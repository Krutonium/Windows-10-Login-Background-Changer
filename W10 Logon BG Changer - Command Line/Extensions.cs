using System.Text.RegularExpressions;

namespace W10_Logon_BG_Changer___Command_Line
{
    public static class Extensions
    {
        public static bool IsHex(this string hex)
        {
            return (Regex.Match(hex, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success);
        }
    }
}
