using Gizmo.HardwareAudit.Classes.Helpers;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.HardwareAuditWPF;
using Gizmo.WPF;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Gizmo.HardwareAudit
{
    public partial class ReportDialog : Window
    {
        private void FillWrapPanelWithIcons(bool useCustomIcon, GizmiComputerHardwareIconsEnum customIcon)
        {
            foreach (var node in GizmoIconHelper.GetIconsByCategory("Item Icons"))
            {
                var btn = new UIButton() { Flat = useCustomIcon == true && node == customIcon ? false : useCustomIcon == false && node == GizmiComputerHardwareIconsEnum.Report ? false : true, Tag = node, Width = 30, Height = 30, Margin = new Thickness(3), Content = new GizmoIcon() { Icon = node, FontSize = 16, IconFontFamily= Application.Current.Resources["GizmoIcon"] as FontFamily } };
                btn.Click += Btn_Click;
                wpIconList.Children.Add(btn);
            }
        }

        public ReportDialog(AppSettings settings, TreeItem root)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            DataContext = new ReportItemSettingsViewModel(root);
            FillWrapPanelWithIcons(false, GizmiComputerHardwareIconsEnum.None);
        }

        public ReportDialog(AppSettings settings, string name, string description, bool useCustomIcon, GizmiComputerHardwareIconsEnum customIcon, TreeItem root, ReportSettings reportSettings)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            DataContext = new ReportItemSettingsViewModel(name, description, useCustomIcon, customIcon, root, reportSettings);
            FillWrapPanelWithIcons(useCustomIcon, customIcon);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TbReportName.Text != string.Empty)
            {
                if ((DataContext as ReportItemSettingsViewModel).UseCustomIcon && (DataContext as ReportItemSettingsViewModel).CustomIcon == GizmiComputerHardwareIconsEnum.None)
                {
                    (DataContext as ReportItemSettingsViewModel).CustomIcon = GizmiComputerHardwareIconsEnum.Report;
                }
                DialogResult = true;
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

        private void TbReportName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TbReportDescription.Focus();
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var node in wpIconList.Children)
            {
                if (node != null)
                {
                    if (node is UIButton)
                    {
                        (node as UIButton).Flat = true;
                    }
                }
            }
            (sender as UIButton).Flat = false;
            (DataContext as ReportItemSettingsViewModel).CustomIcon = (GizmiComputerHardwareIconsEnum)(sender as UIButton).Tag;
        }
    }
}
