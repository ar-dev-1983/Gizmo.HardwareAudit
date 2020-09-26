using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.WPF;
using System;
using System.Windows;
using System.Windows.Input;
namespace Gizmo.HardwareAudit
{
    public partial class ContainerDialog : Window
    {
        public ContainerDialog(AppSettings settings)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            DataContext = new ContainerSettingsViewModel(settings.UserProfiles);
        }

        public ContainerDialog(AppSettings settings, string name, string description, bool useParentId, Guid profileId)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            DataContext = new ContainerSettingsViewModel(settings.UserProfiles, name, description, useParentId, profileId);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TbContainerName.Text != string.Empty)
            {
                DialogResult = cbUseParentUserProfile.IsChecked || CbUserProfile.SelectedIndex != -1;
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

        private void TbContainerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TbContainerDescription.Focus();
            }
        }
    }
}
