using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UINetworkAdapter))]
    public class UINetworkAdapterListBox : ItemsControl
    {
        public UINetworkAdapterListBox()
: base()
        {
            DefaultStyleKey = typeof(UINetworkAdapterListBox);
        }
        static UINetworkAdapterListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UINetworkAdapterListBox), new FrameworkPropertyMetadata(typeof(UINetworkAdapterListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UINetworkAdapter;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UINetworkAdapter() { IconFontFamily = IconFontFamily };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (Items != null && item != null && element is UINetworkAdapter)
            {
                if (Items.IndexOf(item) == Items.Count - 1)
                    (element as UINetworkAdapter).ShowSeparator = false;
            }
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UINetworkAdapterListBox), new UIPropertyMetadata(null));
    }
}
