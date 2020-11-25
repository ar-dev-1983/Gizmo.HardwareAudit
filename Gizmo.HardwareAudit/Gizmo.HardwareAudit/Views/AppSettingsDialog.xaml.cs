using Gizmo.HardwareAudit.Behaviors;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.Services;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.WPF;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.HardwareAudit
{
    /// <summary>
    /// Логика взаимодействия для AddNetworkItemWindow.xaml
    /// </summary>
    public partial class AppSettingsDialog : Window
    {
        private readonly AppSettingsViewModel appsvm;

        public AppSettingsDialog()
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, UIThemeEnum.BlueDark);
            appsvm = new AppSettingsViewModel(new DefaultDialogService(), new SerializationService(), new AppSettings());
            DataContext = appsvm;
        }

        public AppSettingsDialog(AppSettings settings)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            appsvm = new AppSettingsViewModel(new DefaultDialogService(), new SerializationService(), settings);
            DataContext = appsvm;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void PbUserPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UIPasswordBox pBox = sender as UIPasswordBox;
            SecurePasswordAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }

        private void lbUserProfilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                pbUserPassword.SecurePassword = ((sender as ListView).SelectedItem as UserProfile).UserPassword;
            }
        }
    }
}
