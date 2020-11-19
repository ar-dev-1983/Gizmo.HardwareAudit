using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UIMemoryDevice : Control
    {
        public UIMemoryDevice()
: base()
        {
            DefaultStyleKey = typeof(UIMemoryDevice);
        }
        static UIMemoryDevice()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIMemoryDevice), new FrameworkPropertyMetadata(typeof(UIMemoryDevice)));
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIMemoryDevice), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ShowSeparatorProperty = DependencyProperty.Register("ShowSeparator", typeof(bool), typeof(UIMemoryDevice), new FrameworkPropertyMetadata(true));
    }
}
