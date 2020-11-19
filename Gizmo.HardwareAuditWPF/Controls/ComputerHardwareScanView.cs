using Gizmo.HardwareAuditClasses;
using Gizmo.HardwareAuditClasses.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class ComputerHardwareScanView : Control
    {
        public ComputerHardwareScanView()
: base()
        {
            DefaultStyleKey = typeof(ComputerHardwareScanView);
        }
        static ComputerHardwareScanView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComputerHardwareScanView), new FrameworkPropertyMetadata(typeof(ComputerHardwareScanView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }
        public ComputerHardwareScan Item
        {
            get => (ComputerHardwareScan)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public UIViewModeEnum ViewMode
        {
            get => (UIViewModeEnum)GetValue(ViewModeProperty);
            set => SetValue(ViewModeProperty, value);
        }
        public FontFamily Icons
        {
            get => (FontFamily)GetValue(IconsProperty);
            set => SetValue(IconsProperty, value);
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(ComputerHardwareScan), typeof(ComputerHardwareScanView), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register("ViewMode", typeof(UIViewModeEnum), typeof(ComputerHardwareScanView), new UIPropertyMetadata(UIViewModeEnum.All));
        public static readonly DependencyProperty IconsProperty = DependencyProperty.Register("Icons", typeof(FontFamily), typeof(ComputerHardwareScanView), new UIPropertyMetadata(null));
    }
}
