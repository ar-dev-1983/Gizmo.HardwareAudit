using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UIPhysicalDrive : Control
    {
        public UIPhysicalDrive()
: base()
        {
            DefaultStyleKey = typeof(UIPhysicalDrive);
        }
        static UIPhysicalDrive()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIPhysicalDrive), new FrameworkPropertyMetadata(typeof(UIPhysicalDrive)));
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIPhysicalDrive), new UIPropertyMetadata(null));
    }
}
