using Gizmo.HardwareAuditWPF;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Gizmo.HardwareScan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppViewModel appvm;
        Dictionary<string, SolidColorBrush> BrushList = new Dictionary<string, SolidColorBrush>();
        UIThemeEnum AppTheme = UIThemeEnum.BlueDark;
        public MainWindow()
        {
            InitializeComponent();
            appvm = new AppViewModel(Environment.MachineName);
            appvm.OnScanIsFinished += Appvm_OnScanIsFinished;
            appvm.OnErrorCatch += Appvm_OnErrorCatch;
            DataContext = appvm;
        }

        private void Pupulate()
        {
            BrushList.Clear();
            BrushList.Add("Background", ThemeManager.GetResource(AppTheme, "WindowBackgroundBrush") as SolidColorBrush);
            BrushList.Add("Text", ThemeManager.GetResource(AppTheme, "WindowForegroundBrush") as SolidColorBrush);
            BrushList.Add("Header", ThemeManager.GetResource(AppTheme, "RowHighlightBrush") as SolidColorBrush);
        }

        private void Appvm_OnErrorCatch(AppViewModel appViewModel)
        {
            ProgressIcon.Foreground = Brushes.Red;
            InfoErrorTextBlock.Text = appvm.Error.Message;
        }

        private void Appvm_OnScanIsFinished(AppViewModel appViewModel)
        {
            var ESViewMode_animation = new DoubleAnimation(312, TimeSpan.FromMilliseconds(1000));
            ESViewMode_animation.Completed += ESViewMode_animation_Completed;
            ESViewMode.BeginAnimation(UIEnumSwitch.WidthProperty, ESViewMode_animation);
        }

        private void ESViewMode_animation_Completed(object sender, EventArgs e)
        {
            var cgSettings_animation = new DoubleAnimation(126, TimeSpan.FromMilliseconds(403));
            cgSettings_animation.Completed += CgSettings_animation_Completed;
            cgSettings.BeginAnimation(UIControlGroup.WidthProperty, cgSettings_animation);
        }

        private void CgSettings_animation_Completed(object sender, EventArgs e)
        {
            var hsvCurrentScan_animation = new DoubleAnimation(hsvCurrentScan.ActualHeight, TimeSpan.FromMilliseconds(1500));
            hsvCurrentScan.Height = 0;
            svCurrentScan.Visibility = Visibility.Visible;

            hsvCurrentScan.BeginAnimation(HardwareScanView.HeightProperty, hsvCurrentScan_animation);
        }

        #region Theme Changing Events

        private void MiBlueDark_Click(object sender, RoutedEventArgs e)
        {
            AppTheme = UIThemeEnum.BlueDark;
            ThemeManager.ApplyThemeToWindow(this, AppTheme);

        }
        private void MiBlueLight_Click(object sender, RoutedEventArgs e)
        {
            AppTheme = UIThemeEnum.BlueLight;
            ThemeManager.ApplyThemeToWindow(this, AppTheme);
        }
        private void MiPurpleDark_Click(object sender, RoutedEventArgs e)
        {
            AppTheme = UIThemeEnum.PurpleDark;
            ThemeManager.ApplyThemeToWindow(this, AppTheme);
        }
        private void MiPurpleLight_Click(object sender, RoutedEventArgs e)
        {
            AppTheme = UIThemeEnum.PurpleLight;
            ThemeManager.ApplyThemeToWindow(this, AppTheme);
        }

        #endregion

        private void SaveAsPNGFile_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)hsvCurrentScan.ActualWidth, (int)hsvCurrentScan.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(hsvCurrentScan);
            appvm.SaveAsPngFile(rtb);
        }

        private void SaveAsHTMLFile_Click(object sender, RoutedEventArgs e)
        {
            Pupulate();
            appvm.SaveAsHtmlFile(htmlSerializer.Serialize(appvm.HostName, appvm.Scan, BrushList, (UIViewModeEnum)ESViewMode.SelectedValue));

        }
    }
}
