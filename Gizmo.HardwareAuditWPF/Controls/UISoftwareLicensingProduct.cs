using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UISoftwareLicensingProduct : Control
    {
        public UISoftwareLicensingProduct()
: base()
        {
            DefaultStyleKey = typeof(UISoftwareLicensingProduct);
        }
        static UISoftwareLicensingProduct()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISoftwareLicensingProduct), new FrameworkPropertyMetadata(typeof(UISoftwareLicensingProduct)));
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UISoftwareLicensingProduct), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ShowSeparatorProperty = DependencyProperty.Register("ShowSeparator", typeof(bool), typeof(UISoftwareLicensingProduct), new FrameworkPropertyMetadata(true));
    }
}
