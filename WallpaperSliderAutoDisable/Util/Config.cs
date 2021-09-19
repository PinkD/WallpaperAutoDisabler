using System;
using System.Configuration;
using NLog;

namespace WallpaperSliderAutoDisable.util {
    public enum Mode {
        None = 0,
        Auto = 1,
        Shortcut = 2,
    }

    public static class Config {
        private const string _0 = "0";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void UpdateConf(string key, string value) {
            try {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null) {
                    settings.Add(key, value);
                } else {
                    settings[key].Value = value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            } catch (ConfigurationErrorsException) {
                Logger.Warn("Error writing app settings");
            }
        }

        public static Shortcut Shortcut {
            get => Shortcut.FromString(ConfigurationManager.AppSettings["shortcut"] ?? "");
            set => UpdateConf("shortcut", $"{value}");
        }

        public static Mode Mode {
            get => (Mode)Enum.Parse(typeof(Mode), ConfigurationManager.AppSettings["mode"] ?? "None");
            set => UpdateConf("mode", $"{value}");
        }

        public static string Status {
            get => ConfigurationManager.AppSettings["status"] ?? "";
            set => UpdateConf("status", $"{value}");
        }

        private const int DefaultDelay = 5000;

        public static int Delay {
            get {
                var delay = int.Parse(ConfigurationManager.AppSettings["delay"] ?? _0);
                return delay == 0 ? DefaultDelay : delay;
            }
            set => UpdateConf("delay", $"{value}");
        }
    }
}