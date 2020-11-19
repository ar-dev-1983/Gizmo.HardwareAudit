using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UICPUInformation))]
    public class UICPUListBox : ItemsControl
    {
        public UICPUListBox()
: base()
        {
            DefaultStyleKey = typeof(UICPUListBox);
        }
        static UICPUListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UICPUListBox), new FrameworkPropertyMetadata(typeof(UICPUListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UICPUInformation;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            
            return new UICPUInformation() { IconFontFamily = IconFontFamily };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UICPUListBox), new UIPropertyMetadata(null));
    }
}
