using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using WallpaperSliderAutoDisable.util;
using Application = System.Windows.Application;
using Shortcut = WallpaperSliderAutoDisable.util.Shortcut;

namespace WallpaperSliderAutoDisable {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly IWallpaperTool _wallpaperTool = new WallpaperTool();

        private bool _enableAutoSwitch;
        private readonly KeyboardHook _hook = new KeyboardHook();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Assembly _assembly = Assembly.GetExecutingAssembly();


        private static readonly Shortcut DefaultShortcut = new Shortcut(ModifierKeys.Control | ModifierKeys.Alt, Keys.G);

        public MainWindow() {
            InitializeComponent();

            // only register
            var _ = new FullscreenDetectTool(OnFullscreenChanged);
            var status = Config.Status;
            if (status.Length != 0 && status == WallpaperTool.Disabled) {
                _wallpaperTool.SetStatus(false);
                updateStatus();
            }

            AutoModeItem.Header = Properties.Resources.AutoMode;
            ShortcutModeItem.Header = Properties.Resources.ShortcutMode;
            ExitItem.Header = Properties.Resources.Exit;

            Init();
            LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");
        }

        private void Init() {
            _logger.Debug("register shortcut");
            _hook.KeyPressed += OnKeyPressed;
            var shortcut = Config.Shortcut;
            if (!shortcut.Valid()) {
                shortcut = DefaultShortcut;
            }

            var mode = Config.Mode;
            _logger.Debug($"current mode: {mode}");
            switch (mode) {
                case Mode.None:
                // regard as auto mode
                case Mode.Auto:
                    _enableAutoSwitch = true;
                    AutoModeItem.IsChecked = true;
                    break;
                case Mode.Shortcut:
                    _hook.RegisterHotKey(shortcut.MK, shortcut.K);
                    ShortcutModeItem.IsChecked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void updateStatus() {
            var rawStatus = _wallpaperTool.Status();
            var status = Properties.Resources.ResourceManager.GetString(rawStatus);
            Dispatcher.Invoke(new Action(() => { MyNotifyIcon.ToolTipText = status; }));
            _logger.Info(Properties.Resources.switch_status, status);
            using (var stream = Application.GetResourceStream(new Uri($"pack://application:,,,/Icons/{rawStatus}.ico"))?.Stream) {
                if (stream != null) {
                    Dispatcher.Invoke(new Action(() => { MyNotifyIcon.Icon = new Icon(stream); }));
                }
            }

            Config.Status = rawStatus;
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e) {
            _logger.Debug("hotkey pressed");
            _wallpaperTool.Toggle();
            updateStatus();
        }

        private void OnFullscreenChanged(bool fullscreen) {
            if (!_enableAutoSwitch) {
                _logger.Info(Properties.Resources.ignore_fullscreen_event);
                return;
            }

            _logger.Info(Properties.Resources.fullscreen_fmt, fullscreen);
            if (fullscreen) {
                _wallpaperTool.Disable();
            } else {
                _wallpaperTool.Enable();
            }

            updateStatus();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void CheckBox_OnClick(object sender, RoutedEventArgs e) {
            if (AutoModeItem == sender) {
                _enableAutoSwitch = !_enableAutoSwitch;
                AutoModeItem.IsChecked = _enableAutoSwitch;
                ShortcutModeItem.IsChecked = false;
                MyNotifyIcon.ToolTipText = Properties.Resources.AutoMode;
                _hook.Dispose();
                Config.Mode = Mode.Auto;
            } else {
                // ShortcutModeItem
                if (ShortcutModeItem.IsChecked) {
                    return;
                }

                _enableAutoSwitch = false;
                AutoModeItem.IsChecked = false;
                ShortcutModeItem.IsChecked = true;
                MyNotifyIcon.ToolTipText = Properties.Resources.ShortcutMode;
                var shortcut = Config.Shortcut;
                if (!shortcut.Valid()) {
                    shortcut = DefaultShortcut;
                }

                _hook.RegisterHotKey(shortcut.MK, shortcut.K);
                Config.Mode = Mode.Shortcut;
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            MyNotifyIcon.Dispose();
            base.OnClosing(e);
        }
    }
}