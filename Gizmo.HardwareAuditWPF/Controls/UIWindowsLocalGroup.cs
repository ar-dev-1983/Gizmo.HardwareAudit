using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UIWindowsLocalGroup : Control
    {
        public UIWindowsLocalGroup()
: base()
        {
            DefaultStyleKey = typeof(UIWindowsLocalGroup);
        }
        static UIWindowsLocalGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIWindowsLocalGroup), new FrameworkPropertyMetadata(typeof(UIWindowsLocalGroup)));
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
        public bool ShowSeparator
        {
            get => (bool)GetValue(ShowSeparatorProperty);
            set => SetValue(ShowSeparatorProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIWindowsLocalGroup), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ShowSeparatorProperty = DependencyProperty.Register("ShowSeparator", typeof(bool), typeof(UIWindowsLocalGroup), new FrameworkPropertyMetadata(true));
    }
}
