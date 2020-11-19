using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UIPhysicalDrive))]
    public class UIPhysicalDriveListBox : ItemsControl
    {
        public UIPhysicalDriveListBox()
: base()
        {
            DefaultStyleKey = typeof(UIPhysicalDriveListBox);
        }
        static UIPhysicalDriveListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIPhysicalDriveListBox), new FrameworkPropertyMetadata(typeof(UIPhysicalDriveListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UIPhysicalDrive;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UIPhysicalDrive() { IconFontFamily = IconFontFamily };
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIPhysicalDriveListBox), new UIPropertyMetadata(null));
    }
}
