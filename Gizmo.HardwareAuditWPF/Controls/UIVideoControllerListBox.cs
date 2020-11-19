using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UIVideoController))]
    public class UIVideoControllerListBox : ItemsControl
    {
        public UIVideoControllerListBox()
: base()
        {
            DefaultStyleKey = typeof(UIVideoControllerListBox);
        }
        static UIVideoControllerListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIVideoControllerListBox), new FrameworkPropertyMetadata(typeof(UIVideoControllerListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UIVideoController;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UIVideoController() { IconFontFamily = IconFontFamily };
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
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIVideoControllerListBox), new UIPropertyMetadata(null));
    }
}
