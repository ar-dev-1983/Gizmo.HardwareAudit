using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UICPUInformation : Control
    {
        public UICPUInformation()
: base()
        {
            DefaultStyleKey = typeof(UICPUInformation);
        }
        static UICPUInformation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UICPUInformation), new FrameworkPropertyMetadata(typeof(UICPUInformation)));
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

        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UICPUInformation), new UIPropertyMetadata(null));
    }
}
