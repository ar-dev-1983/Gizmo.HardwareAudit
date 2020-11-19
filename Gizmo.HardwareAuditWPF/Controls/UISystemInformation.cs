using Gizmo.HardwareAuditClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class UISystemInformation : Control
    {
        public UISystemInformation()
: base()
        {
            DefaultStyleKey = typeof(UISystemInformation);
        }
        static UISystemInformation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UISystemInformation), new FrameworkPropertyMetadata(typeof(UISystemInformation)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public SystemInformation SystemInfo
        {
            get => (SystemInformation)GetValue(SystemInfoProperty);
            set => SetValue(SystemInfoProperty, value);
        }
        public MotherBoardInformation MotherBoardInfo
        {
            get => (MotherBoardInformation)GetValue(MotherBoardInfoProperty);
            set => SetValue(MotherBoardInfoProperty, value);
        }

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }

        public static readonly DependencyProperty SystemInfoProperty = DependencyProperty.Register("SystemInfo", typeof(SystemInformation), typeof(UISystemInformation), new UIPropertyMetadata(null));
        public static readonly DependencyProperty MotherBoardInfoProperty = DependencyProperty.Register("MotherBoardInfo", typeof(MotherBoardInformation), typeof(UISystemInformation), new UIPropertyMetadata(null));
        
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(UISystemInformation), new UIPropertyMetadata(null));
    }
}
