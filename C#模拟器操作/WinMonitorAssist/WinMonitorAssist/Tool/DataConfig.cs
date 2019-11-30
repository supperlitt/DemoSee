using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace WinMonitorAssist
{
    public class DataConfig
    {
        private static string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data.config");

        public static void SaveSourcePath(string value)
        {
            SaveConfig("source_path", value);
        }

        public static string ReadSourcePath()
        {
            return ReadConfig("source_path");
        }

        private static void SaveConfig(string key, string value)
        {
            lock (configPath)
            {
                try
                {
                    ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                    map.ExeConfigFilename = configPath;
                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings.AllKeys.Contains(key))
                    {
                        config.AppSettings.Settings[key].Value = value;
                        config.Save();
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        private static string ReadConfig(string key)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configPath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            if (config.AppSettings.Settings.AllKeys.Contains(key))
            {
                return config.AppSettings.Settings[key].Value;
            }

            return string.Empty;
        }
    }
}
