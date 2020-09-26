using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.WPF;
using System;
using System.Windows;
using System.Windows.Input;
namespace Gizmo.HardwareAudit
{
    public partial class ComputerDialog : Window
    {
        public ComputerDialog(AppSettings settings)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            Name = string.Empty;
            DataContext = new ComputerSettingsViewModel(settings.UserProfiles);
        }

        public ComputerDialog(AppSettings settings, string name, string desc, string fqdn, string address, bool useParentId, Guid profileId)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            DataContext = new ComputerSettingsViewModel(settings.UserProfiles, name, desc, fqdn, address, useParentId, profileId);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TbComputerName.Text != string.Empty)
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
                //TbContainerDescription.Focus();
            }
        }

        private void TbContainerDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSave.Focus();
            }
        }
    }
}
