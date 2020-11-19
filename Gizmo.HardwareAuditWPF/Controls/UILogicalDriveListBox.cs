using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UILogicalDrive))]
    public class UILogicalDriveListBox : ItemsControl
    {
        public UILogicalDriveListBox()
: base()
        {
            DefaultStyleKey = typeof(UILogicalDriveListBox);
        }
        static UILogicalDriveListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UILogicalDriveListBox), new FrameworkPropertyMetadata(typeof(UILogicalDriveListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UILogicalDrive;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UILogicalDrive() { IconFontFamily = IconFontFamily };
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UILogicalDriveListBox), new UIPropertyMetadata(null));
    }
}
