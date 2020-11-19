using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
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
        #region Private Methods
        private void Pupulate()
        {
            var b = ThemeManager.GetResource(AppTheme, "WindowBackgroundBrush") as SolidColorBrush;
            var t = ThemeManager.GetResource(AppTheme, "WindowForegroundBrush") as SolidColorBrush;
            var h = ThemeManager.GetResource(AppTheme, "RowHighlightBrush") as SolidColorBrush;

            BrushList.Clear();
            BrushList.Add("Background", new fakeColor(b.Color.A, b.Color.R, b.Color.G, b.Color.B));
            BrushList.Add("Text", new fakeColor(t.Color.A, t.Color.R, t.Color.G, t.Color.B));
            BrushList.Add("Header", new fakeColor(h.Color.A, h.Color.R, h.Color.G, h.Color.B));
        }
        #endregion

        #region Private Properties
        private AppViewModel appvm;
        private Dictionary<string, fakeColor> BrushList = new Dictionary<string, fakeColor>();
        private UIThemeEnum AppTheme = UIThemeEnum.BlueDark;
        private DoubleAnimation ShowControls0Animation;
        private DoubleAnimation ShowControls1Animation;
        private DoubleAnimation ShowControls2Animation;
        private DoubleAnimation ShowControls3Animation;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            ShowControls0Animation = new DoubleAnimation(0, TimeSpan.FromMilliseconds(150));
            ShowControls1Animation = new DoubleAnimation(312, TimeSpan.FromMilliseconds(900));
            ShowControls2Animation = new DoubleAnimation(126, TimeSpan.FromMilliseconds(300));
            ShowControls3Animation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(150));
            ShowControls0Animation.Completed += ShowControls0Animation_Completed;
            ShowControls1Animation.Completed += ShowControls1Animation_Completed;
            ShowControls2Animation.Completed += ShowControls2Animation_Completed;

            appvm = new AppViewModel(Environment.MachineName);
            appvm.OnScanIsFinished += Appvm_OnScanIsFinished;
            appvm.OnErrorCatch += Appvm_OnErrorCatch;
            DataContext = appvm;
        }

        #region Event Handlers
        private void Appvm_OnErrorCatch(AppViewModel appViewModel)
        {
            ProgressIcon.Foreground = Brushes.Red;
            InfoErrorTextBlock.Text = appvm.Error.Message;
        }

        private void Appvm_OnScanIsFinished(AppViewModel appViewModel)
        {
            grScanLogo.BeginAnimation(OpacityProperty, ShowControls0Animation);
        }

        private void ShowControls0Animation_Completed(object sender, EventArgs e)
        {
            grScanLogo.Visibility = Visibility.Collapsed;
            grData.Visibility = Visibility.Visible;
            ESViewMode.BeginAnimation(WidthProperty, ShowControls1Animation);
        }
        private void ShowControls1Animation_Completed(object sender, EventArgs e)
        {
            cgSettings.BeginAnimation(WidthProperty, ShowControls2Animation);
        }
        private void ShowControls2Animation_Completed(object sender, EventArgs e)
        {
            hsvCurrentScan.Opacity = 0;
            svCurrentScan.Visibility = Visibility.Visible;
            hsvCurrentScan.BeginAnimation(OpacityProperty, ShowControls3Animation);
        }
        #endregion

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

        #region Save as.. Events
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
        #endregion

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow(AppTheme);
            aboutWindow.ShowDialog();
        }
    }
}
