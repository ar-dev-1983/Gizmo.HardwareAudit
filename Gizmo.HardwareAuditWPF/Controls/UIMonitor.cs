using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UIMonitor : Control
    {
        public UIMonitor()
: base()
        {
            DefaultStyleKey = typeof(UIMonitor);
        }
        static UIMonitor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIMonitor), new FrameworkPropertyMetadata(typeof(UIMonitor)));
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIMonitor), new UIPropertyMetadata(null));
    }
}
