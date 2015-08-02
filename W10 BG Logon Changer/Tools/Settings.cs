using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W10_BG_Logon_Changer.Tools
{
    public static class Settings
    {
        private static readonly Dictionary<string, object> SettingsObject; 
        static Settings()
        {
            if (SettingsObject == null)
            {
                if (!File.Exists(Config.SettingsFilePath))
                    SettingsObject = new Dictionary<string, object>();
            }
            else
            {
                //serlize are settings file
            }
        }

        public static T Get<T>(string key, T @default) where T : class
        {
            if (SettingsObject[key] != null)
            {
                return (T) SettingsObject[key];
            }

            return @default;
        }

        public static void Set<T>(string key, T @value) where T : class
        {
            if (SettingsObject[key] != null)
            {
                var test = SettingsObject[key].GetType();

                if (@value.GetType() == test)
                {
                    SettingsObject[key] = @value;
                }

                return;
            }

            SettingsObject.Add(key, @value);
        }
    }
}
