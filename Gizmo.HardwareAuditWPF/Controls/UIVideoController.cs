using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UIVideoController : Control
    {
        public UIVideoController()
: base()
        {
            DefaultStyleKey = typeof(UIVideoController);
        }
        static UIVideoController()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIVideoController), new FrameworkPropertyMetadata(typeof(UIVideoController)));
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIVideoController), new UIPropertyMetadata(null));
    }
}
