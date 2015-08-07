using System;
using System.IO;

namespace SharedLibrary
{
    public static class Config
    {
        public static readonly string PriFileName = "Windows.UI.Logon.pri";
        public static readonly string BakPriFileName = PriFileName + ".bak";
        public static readonly string CurrentImage = "current.img";
        public static readonly string SettingsFile = "Settings.bin";

        public static readonly string LoginFolder = Path.Combine(Environment.GetEnvironmentVariable("windir"),
            "SystemResources", "Windows.UI.Logon");

        public static readonly string PriFileLocation = Path.Combine(LoginFolder, PriFileName);
        public static readonly string BakPriFileLocation = Path.Combine(LoginFolder, BakPriFileName);
        public static readonly string CurrentImageLocation = Path.Combine(LoginFolder, CurrentImage);

        public static readonly string SettingsFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "W10LoginChanger",
                SettingsFile);
    }
}