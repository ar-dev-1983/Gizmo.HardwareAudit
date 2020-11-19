using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UISoftwareLicensingProduct))]
    public class UISoftwareLicensingProductListBox : ItemsControl
    {
        public UISoftwareLicensingProductListBox()
: base()
        {
            DefaultStyleKey = typeof(UISoftwareLicensingProductListBox);
        }
        static UISoftwareLicensingProductListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISoftwareLicensingProductListBox), new FrameworkPropertyMetadata(typeof(UISoftwareLicensingProductListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UISoftwareLicensingProduct;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UISoftwareLicensingProduct() { IconFontFamily = IconFontFamily };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (Items != null && item != null && element is UISoftwareLicensingProduct)
            {
                if (Items.IndexOf(item) == Items.Count - 1)
                    (element as UISoftwareLicensingProduct).ShowSeparator = false;
            }
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UISoftwareLicensingProductListBox), new UIPropertyMetadata(null));
    }
}
