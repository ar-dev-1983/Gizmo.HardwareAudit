using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UIPrinter : Control
    {
        public UIPrinter()
: base()
        {
            DefaultStyleKey = typeof(UIPrinter);
        }
        static UIPrinter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIPrinter), new FrameworkPropertyMetadata(typeof(UIPrinter)));
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIPrinter), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ShowSeparatorProperty = DependencyProperty.Register("ShowSeparator", typeof(bool), typeof(UIPrinter), new FrameworkPropertyMetadata(true));
    }
}
