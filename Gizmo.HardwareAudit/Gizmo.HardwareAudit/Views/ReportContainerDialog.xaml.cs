using Gizmo.HardwareAudit.Classes.Helpers;
using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.HardwareAudit.Views;
using Gizmo.WPF;
using System.Windows;
using System.Windows.Input;
namespace Gizmo.HardwareAudit
{
    public partial class ReportContainerDialog : Window
    {
        private void FillWrapPanelWithIcons(bool useCustomIcon, GizmoIconEnum customIcon)
        {
            foreach (var node in GizmoIconHelper.GetIconsByCategory("Container Icons"))
            {
                var btn = new UIButton() { Flat = useCustomIcon == true && node == customIcon ? false : useCustomIcon == false && node == GizmoIconEnum.Container ? false : true, Tag = node, Width = 30, Height = 30, Margin = new Thickness(3), Content = new GizmoIcon() { Icon = node, FontSize = 16 } };
                btn.Click += Btn_Click;
                wpIconList.Children.Add(btn);
            }
        }

        public ReportContainerDialog(AppSettings settings)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            DataContext = new ReportContainerSettingsViewModel();
            FillWrapPanelWithIcons(false, GizmoIconEnum.None);

        }

        public ReportContainerDialog(AppSettings settings, string name, string description, bool useCustomIcon, GizmoIconEnum customIcon)
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
            ThemeManager.ApplyThemeToWindow(this, settings.Theme);
            DataContext = new ReportContainerSettingsViewModel(name, description, useCustomIcon, customIcon);
            FillWrapPanelWithIcons(useCustomIcon, customIcon);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TbContainerName.Text != string.Empty)
            {
                if ((DataContext as ReportContainerSettingsViewModel).UseCustomIcon && (DataContext as ReportContainerSettingsViewModel).CustomIcon == GizmoIconEnum.None)
                {
                    (DataContext as ReportContainerSettingsViewModel).CustomIcon = GizmoIconEnum.Container;
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

        private void TbContainerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TbContainerDescription.Focus();
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
            (DataContext as ReportContainerSettingsViewModel).CustomIcon = (GizmoIconEnum)(sender as UIButton).Tag;
        }
    }
}
