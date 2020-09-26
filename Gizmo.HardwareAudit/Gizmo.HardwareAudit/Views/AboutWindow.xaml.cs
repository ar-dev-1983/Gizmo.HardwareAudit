using Gizmo.HardwareAudit.Models;
using Gizmo.WPF;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace Gizmo.HardwareAudit
{
    public partial class AboutWindow : Window
    {
        public AboutWindow(AppSettings appSettings)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, appSettings.Theme);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
