using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
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
                    Thread.Sleep(500);
                    DetectFullscreen();
                }
            };
            worker.RunWorkerAsync();
        }

        private void DetectFullscreen() {
            var fc = IsFullscreen();
            Console.WriteLine(Resources.fullscreen_fmt, fc);
            if (fc == _fullscreen) {
                return;
            }

            _fullscreen = fc;
            _onFullscreenChange(fc);
        }

        private static bool IsFullscreen() {
            var ret = SHQueryUserNotificationState(out var state);
            if (ret != 0) {
                Console.WriteLine(Resources.get_user_notification_state, ret);
            }

            Console.WriteLine(state);

            return state == QueryUserNotificationStatState.QUNS_BUSY || state == QueryUserNotificationStatState.QUNS_RUNNING_D3D_FULL_SCREEN;
        }
    }
}