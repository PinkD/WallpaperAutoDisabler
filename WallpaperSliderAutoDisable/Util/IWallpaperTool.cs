using NLog;
using WallpaperSliderAutoDisable.Properties;

namespace WallpaperSliderAutoDisable.util {
    public interface IWallpaperTool {
        bool Disable();
        bool Enable();
        bool Toggle();
        string Status();
        void SetStatus(bool enabled);
    }

    internal class WallpaperTool : IWallpaperTool {
        public const string Enabled = "Enabled";
        public const string Disabled = "Disabled";

        private const int S_OK = 0;

        private readonly IDesktopWallpaper _wallpaper;
        private bool _enable;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();


        public WallpaperTool() {
            var dwc = new DesktopWallpaperClass();
            _wallpaper = (IDesktopWallpaper)dwc;
            _enable = true;
        }

        public bool Disable() {
            var result = _wallpaper.Enable(false);
            if (result != S_OK) {
                return false;
            }

            _enable = false;
            _logger.Info(Resources.disable_wallpaper);
            return true;
        }

        public bool Enable() {
            var result = _wallpaper.Enable(true);
            if (result != S_OK) {
                return false;
            }

            _enable = true;
            _logger.Info(Resources.enable_wallpaper);
            return true;
        }

        public bool Toggle() {
            return _enable ? Disable() : Enable();
        }

        public string Status() {
            return _enable ? Enabled : Disabled;
        }

        public void SetStatus(bool enable) {
            _enable = enable;
        }
    }
}