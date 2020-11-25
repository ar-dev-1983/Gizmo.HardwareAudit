using Gizmo.HardwareAuditClasses.Helpers;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
        private DoubleAnimation HideLogoAnimation;
        private DoubleAnimation ShowControlsAnimation;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            HideLogoAnimation = new DoubleAnimation(0, TimeSpan.FromMilliseconds(350));
            ShowControlsAnimation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(350));
            HideLogoAnimation.Completed += HideLogoAnimation_Completed;

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
            grScanLogo.BeginAnimation(OpacityProperty, HideLogoAnimation);
        }

        private void HideLogoAnimation_Completed(object sender, EventArgs e)
        {
            grScanLogo.Visibility = Visibility.Hidden;
            grData.Opacity = 0;
            grData.Visibility = Visibility.Visible;
            grData.BeginAnimation(OpacityProperty, ShowControlsAnimation);
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
        private void UIPopupButton_PopupOpened(object sender, RoutedEventArgs e)
        {
            hsvCurrentScan.UpdateLayout();
            if ((int)hsvCurrentScan.ActualHeight != 0 && (int)hsvCurrentScan.ActualWidth != 0)
            {
                SaveAsPNGFile.IsEnabled = true;
            }
            else
            {
                SaveAsPNGFile.IsEnabled = false;
            }
        }
        private void SaveAsPNGFile_Click(object sender, RoutedEventArgs e)
        {
            hsvCurrentScan.UpdateLayout();
            if ((int)hsvCurrentScan.ActualHeight != 0 && (int)hsvCurrentScan.ActualWidth != 0)
            {
                hsvCurrentScan.UpdateLayout();
                var previous_background = hsvCurrentScan.Background;
                hsvCurrentScan.Background = Background;
                appvm.SaveAsPngFile(VisualHelper.SnapShotPNG(hsvCurrentScan));
                hsvCurrentScan.Background = previous_background;
            }
        }
        private void SaveAsHTMLFile_Click(object sender, RoutedEventArgs e)
        {
            Pupulate();
            appvm.SaveAsHtmlFile(htmlSerializer.Serialize(appvm.HostName, appvm.Scan, BrushList, ESViewMode.SelectedItems));
        }
        #endregion

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow(AppTheme);
            aboutWindow.ShowDialog();
        }

    }
}
