using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UIMemoryDevice))]
    public class UIMemoryDeviceListBox : ItemsControl
    {
        public UIMemoryDeviceListBox()
: base()
        {
            DefaultStyleKey = typeof(UIMemoryDeviceListBox);
        }
        static UIMemoryDeviceListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIMemoryDeviceListBox), new FrameworkPropertyMetadata(typeof(UIMemoryDeviceListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UIMemoryDevice;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UIMemoryDevice() { IconFontFamily = IconFontFamily };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (Items != null && item != null && element is UIMemoryDevice)
            {
                if (Items.IndexOf(item) == Items.Count - 1)
                    (element as UIMemoryDevice).ShowSeparator = false;
            }
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIMemoryDeviceListBox), new UIPropertyMetadata(null));
    }
}
