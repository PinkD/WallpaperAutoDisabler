using System;
using System.ComponentModel;
using System.Windows;
using WallpaperSliderAutoDisable.util;

namespace WallpaperSliderAutoDisable {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly IWallpaperTool _wallpaperTool;
        private readonly FullscreenDetectTool _fullscreenDetectTool;

        private bool _enableAutoSwitch;

        public string ToolTipText { get; set; }


        public MainWindow() {
            InitializeComponent();

            _wallpaperTool = new WallpaperTool();
            _enableAutoSwitch = true;
            _fullscreenDetectTool = new FullscreenDetectTool(OnFullscreenChanged);
            MyNotifyIcon.ToolTipText = Properties.Resources.Enabled;
            EnableItem.Header = Properties.Resources.Disable;
            ExitItem.Header = Properties.Resources.Exit;
        }

        private void OnFullscreenChanged(bool fullscreen) {
            if (!_enableAutoSwitch) {
                Console.WriteLine(Properties.Resources.ignore_fullscreen_event);
                return;
            }

            Console.WriteLine(Properties.Resources.fullscreen_fmt, fullscreen);
            if (fullscreen) {
                _wallpaperTool.Disable();
            } else {
                _wallpaperTool.Enable();
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void CheckBox_OnClick(object sender, RoutedEventArgs e) {
            _enableAutoSwitch = !_enableAutoSwitch;
            var text = _enableAutoSwitch ? Properties.Resources.Enabled : Properties.Resources.Disabled;
            MyNotifyIcon.ToolTipText = text;
            text = _enableAutoSwitch ? Properties.Resources.Disable : Properties.Resources.Enable;
            EnableItem.Header = text;
        }

        protected override void OnClosing(CancelEventArgs e) {
            MyNotifyIcon.Dispose();
            base.OnClosing(e);
        }
    }
}