using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.WPF;
using System.Net;
using System.Windows;

namespace Gizmo.HardwareAudit
{
    public partial class ActiveDirectoryDialog : Window
    {
        public ActiveDirectoryDialog(AppSettings appSettings)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, appSettings.Theme);
            DataContext = new DomainDiscoverySettingsViewModel(appSettings.UserProfiles);
            EwMode.SelectedItem = (DataContext as DomainDiscoverySettingsViewModel).Settings.Mode;
        }
        public ActiveDirectoryDialog(AppSettings appSettings, DomainDiscoverySettings domainDiscoverySettings)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, appSettings.Theme);
            EwMode.IsHitTestVisible = false;
            TbDomainName.IsReadOnly = true;
            DataContext = new DomainDiscoverySettingsViewModel(appSettings.UserProfiles, domainDiscoverySettings);
            EwMode.SelectedItem = (DataContext as DomainDiscoverySettingsViewModel).Settings.Mode;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TbDomainName.Text != string.Empty)
            {
                if (!IPAddress.TryParse(TbDomainName.Text, out _))
                {
                    DialogResult = CbUserProfile.SelectedIndex != -1;
                    (DataContext as DomainDiscoverySettingsViewModel).Settings.Mode = (DomainDiscoveryModeEnum)EwMode.LastSelectedItem;
                }
                else
                {
                    DialogResult = false;
                }
            }
            else
            {
                DialogResult = false;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
