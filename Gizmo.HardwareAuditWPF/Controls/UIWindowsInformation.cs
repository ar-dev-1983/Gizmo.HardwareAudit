using Gizmo.HardwareAuditClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UIWindowsInformation : Control
    {
        public UIWindowsInformation()
: base()
        {
            DefaultStyleKey = typeof(UIWindowsInformation);
        }
        static UIWindowsInformation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIWindowsInformation), new FrameworkPropertyMetadata(typeof(UIWindowsInformation)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public WindowsInformation Info
        {
            get => (WindowsInformation)GetValue(InfoProperty);
            set => SetValue(InfoProperty, value);
        }

        public string LoggedInUser
        {
            get => (string)GetValue(LoggedInUserProperty);
            set => SetValue(LoggedInUserProperty, value);
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }

        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register("Info", typeof(WindowsInformation), typeof(UIWindowsInformation), new UIPropertyMetadata(null));
        public static readonly DependencyProperty LoggedInUserProperty = DependencyProperty.Register("LoggedInUser", typeof(string), typeof(UIWindowsInformation), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UIWindowsInformation), new UIPropertyMetadata(null));
    }
}
