using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(UIPrinter))]
    public class UIPrinterListBox : ItemsControl
    {
        public UIPrinterListBox()
: base()
        {
            DefaultStyleKey = typeof(UIPrinterListBox);
        }
        static UIPrinterListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIPrinterListBox), new FrameworkPropertyMetadata(typeof(UIPrinterListBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UIPrinter;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UIPrinter() { IconFontFamily = IconFontFamily };
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (Items != null && item != null && element is UIPrinter)
            {
                if (Items.IndexOf(item) == Items.Count - 1)
                    (element as UIPrinter).ShowSeparator = false;
            }
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIPrinterListBox), new UIPropertyMetadata(null));
    }
}
