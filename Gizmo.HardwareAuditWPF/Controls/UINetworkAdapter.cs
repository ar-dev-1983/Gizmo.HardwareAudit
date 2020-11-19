using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UINetworkAdapter : Control
    {
        public UINetworkAdapter()
: base()
        {
            DefaultStyleKey = typeof(UINetworkAdapter);
        }
        static UINetworkAdapter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UINetworkAdapter), new FrameworkPropertyMetadata(typeof(UINetworkAdapter)));
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UINetworkAdapter), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ShowSeparatorProperty = DependencyProperty.Register("ShowSeparator", typeof(bool), typeof(UINetworkAdapter), new FrameworkPropertyMetadata(true));
    }
}
