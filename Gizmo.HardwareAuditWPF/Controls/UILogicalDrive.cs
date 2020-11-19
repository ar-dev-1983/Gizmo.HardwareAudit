using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UILogicalDrive : Control
    {
        public UILogicalDrive()
: base()
        {
            DefaultStyleKey = typeof(UILogicalDrive);
        }
        static UILogicalDrive()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UILogicalDrive), new FrameworkPropertyMetadata(typeof(UILogicalDrive)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UILogicalDrive), new UIPropertyMetadata(null));
    }
}
