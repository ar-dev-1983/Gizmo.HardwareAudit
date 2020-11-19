using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UIWindowsLocalUser))]
    public class UIWindowsLocalUserListBox : ItemsControl
    {
        public UIWindowsLocalUserListBox()
: base()
        {
            DefaultStyleKey = typeof(UIWindowsLocalUserListBox);
        }
        static UIWindowsLocalUserListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIWindowsLocalUserListBox), new FrameworkPropertyMetadata(typeof(UIWindowsLocalUserListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UIWindowsLocalUser;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UIWindowsLocalUser() { IconFontFamily = IconFontFamily };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (Items != null && item != null && element is UIWindowsLocalUser)
            {
                if (Items.IndexOf(item) == Items.Count - 1)
                    (element as UIWindowsLocalUser).ShowSeparator = false;
            }
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIWindowsLocalUserListBox), new UIPropertyMetadata(null));
    }
}
