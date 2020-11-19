using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UIWindowsLocalUser : Control
    {
        public UIWindowsLocalUser()
: base()
        {
            DefaultStyleKey = typeof(UIWindowsLocalUser);
        }
        static UIWindowsLocalUser()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIWindowsLocalUser), new FrameworkPropertyMetadata(typeof(UIWindowsLocalUser)));
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIWindowsLocalUser), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ShowSeparatorProperty = DependencyProperty.Register("ShowSeparator", typeof(bool), typeof(UIWindowsLocalUser), new FrameworkPropertyMetadata(true));
    }
}
