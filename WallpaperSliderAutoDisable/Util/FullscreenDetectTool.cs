using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using NLog;
using WallpaperSliderAutoDisable.Properties;

namespace WallpaperSliderAutoDisable.util {
    public class FullscreenDetectTool {
        private enum QueryUserNotificationStatState {
            QUNS_NOT_PRESENT,
            QUNS_BUSY,
            QUNS_RUNNING_D3D_FULL_SCREEN,
            QUNS_PRESENTATION_MODE,
            QUNS_ACCEPTS_NOTIFICATIONS,
            QUNS_QUIET_TIME,
            QUNS_APP
        }

        [DllImport("shell32.dll")]
        private static extern int SHQueryUserNotificationState(
            out QueryUserNotificationStatState state
        );

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public delegate void OnFullscreenChange(bool fullscreen);

        private bool _fullscreen;
        private readonly OnFullscreenChange _onFullscreenChange;


        public FullscreenDetectTool(OnFullscreenChange onFullscreenChange) {
            _onFullscreenChange = onFullscreenChange;
            _fullscreen = false;
            var worker = new BackgroundWorker();
            // detect state every 5 second
            worker.DoWork += (sender, args) => {
                while (true) {
                    Thread.Sleep(Config.Delay);
                    DetectFullscreen();
                }
            };
            worker.RunWorkerAsync();
        }

        private void DetectFullscreen() {
            var fc = IsFullscreen();
            Logger.Debug(Resources.fullscreen_fmt, fc);
            if (fc == _fullscreen) {
                return;
            }

            _fullscreen = fc;
            _onFullscreenChange(fc);
        }

        private static bool IsFullscreen() {
            var ret = SHQueryUserNotificationState(out var state);
            if (ret != 0) {
                Logger.Debug(Resources.get_user_notification_state, ret);
            }

            Logger.Debug(state);

            switch (state) {
                case QueryUserNotificationStatState.QUNS_BUSY:
                case QueryUserNotificationStatState.QUNS_PRESENTATION_MODE:
                case QueryUserNotificationStatState.QUNS_RUNNING_D3D_FULL_SCREEN:
                    return true;
                default:
                    return false;
            }
        }
    }
}