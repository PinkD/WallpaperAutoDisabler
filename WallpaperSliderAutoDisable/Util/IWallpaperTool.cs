using WallpaperSliderAutoDisable.Properties;

namespace WallpaperSliderAutoDisable.util {
    public interface IWallpaperTool {
        bool Disable();
        bool Enable();
        bool Toggle();
    }

    internal class WallpaperTool : IWallpaperTool {
        private const int S_OK = 0;

        private readonly IDesktopWallpaper _wallpaper;
        private bool _enable;


        public WallpaperTool() {
            var dwc = new DesktopWallpaperClass();
            _wallpaper = (IDesktopWallpaper) dwc;
            _enable = true;
        }

        public bool Disable() {
            var result = _wallpaper.Enable(false);
            if (result != S_OK) {
                return false;
            }

            _enable = false;
            System.Console.WriteLine(Resources.disable_wallpaper);
            return true;
        }

        public bool Enable() {
            var result = _wallpaper.Enable(true);
            if (result != S_OK) {
                return false;
            }

            _enable = true;
            System.Console.WriteLine(Resources.enable_wallpaper);
            return true;
        }

        public bool Toggle() {
            return _enable ? Disable() : Enable();
        }
    }
}