using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UIWindowsLocalUser))]
    public class UIWindowsLocalGroupListBox : ItemsControl
    {
        public UIWindowsLocalGroupListBox()
: base()
        {
            DefaultStyleKey = typeof(UIWindowsLocalGroupListBox);
        }
        static UIWindowsLocalGroupListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIWindowsLocalGroupListBox), new FrameworkPropertyMetadata(typeof(UIWindowsLocalGroupListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UIWindowsLocalGroup;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UIWindowsLocalGroup() { IconFontFamily = IconFontFamily };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (Items != null && item != null && element is UIWindowsLocalGroup)
            {
                if (Items.IndexOf(item) == Items.Count - 1)
                    (element as UIWindowsLocalGroup).ShowSeparator = false;
            }
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIWindowsLocalGroupListBox), new UIPropertyMetadata(null));
    }
}
