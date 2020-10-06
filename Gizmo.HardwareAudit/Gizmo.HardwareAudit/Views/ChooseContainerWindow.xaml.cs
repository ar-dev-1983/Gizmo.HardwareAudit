using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.WPF;
using System.Windows;

namespace Gizmo.HardwareAudit
{
    public partial class ChooseContainerWindow : Window
    {
        private readonly ChooseContainerViewModel ccvm;

        public ChooseContainerWindow(AppSettings appSettings, TreeItem treeItem)
        {
            InitializeComponent();
            ccvm = new ChooseContainerViewModel(treeItem);
            DataContext = ccvm;
            Owner = Application.Current.MainWindow;
            ThemeManager.ApplyThemeToWindow(this, appSettings.Theme);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ccvm != null)
            {
                if (ccvm.SelectedContainer != null)
                {
                    if (ccvm.SelectedContainer.IsTrueContainer)
                    {
                        DialogResult = true;
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
