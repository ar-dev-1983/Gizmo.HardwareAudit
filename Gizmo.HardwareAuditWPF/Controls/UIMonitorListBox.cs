using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UIMonitor))]
    public class UIMonitorListBox : ItemsControl
    {
        public UIMonitorListBox()
: base()
        {
            DefaultStyleKey = typeof(UIMonitorListBox);
        }
        static UIMonitorListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIMonitorListBox), new FrameworkPropertyMetadata(typeof(UIMonitorListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UIMonitor;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UIMonitor() { IconFontFamily = IconFontFamily };
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIMonitorListBox), new UIPropertyMetadata(null));
    }
}
