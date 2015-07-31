using System;
using System.IO;

namespace W10_BG_Logon_Changer
{
    public static class Config
    {
        public static readonly string PriFileName = "Windows.UI.Logon.pri";
        public static readonly string BakPriFileName = PriFileName + ".bak";

        public static readonly string LogonFolder = Path.Combine(Environment.GetEnvironmentVariable("windir"), "SystemResources", "Windows.UI.Logon");
        public static readonly string PriFileLocation = Path.Combine(LogonFolder, PriFileName);
        public static readonly string BakPriFileLocation = Path.Combine(LogonFolder, BakPriFileName);
    }
}
