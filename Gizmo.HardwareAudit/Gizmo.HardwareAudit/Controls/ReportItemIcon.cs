using Gizmo.HardwareAudit.Enums;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.HardwareAudit.Controls
{
    public class ReportItemIcon : ContentControl
    {
        public ReportItemIcon()
: base()
        {
            DefaultStyleKey = typeof(ReportItemIcon);
        }
        static ReportItemIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReportItemIcon), new FrameworkPropertyMetadata(typeof(ReportItemIcon)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        public ReportItemTypeEnum ItemType
        {
            get => (ReportItemTypeEnum)GetValue(ItemTypeProperty);
            set => SetValue(ItemTypeProperty, value);
        }
        public bool UseCustomIcon
        {
            get => (bool)GetValue(UseCustomIconProperty);
            set => SetValue(UseCustomIconProperty, value);
        }
        public GizmoIconEnum CustomIcon
        {
            get => (GizmoIconEnum)GetValue(CustomIconProperty);
            set => SetValue(CustomIconProperty, value);
        }
        public static readonly DependencyProperty ItemTypeProperty = DependencyProperty.Register("ItemType", typeof(ReportItemTypeEnum), typeof(ReportItemIcon), new UIPropertyMetadata(ReportItemTypeEnum.None));
        public static readonly DependencyProperty UseCustomIconProperty = DependencyProperty.Register("UseCustomIcon", typeof(bool), typeof(ReportItemIcon), new UIPropertyMetadata(false));
        public static readonly DependencyProperty CustomIconProperty = DependencyProperty.Register("CustomIcon", typeof(GizmoIconEnum), typeof(ReportItemIcon), new UIPropertyMetadata(GizmoIconEnum.None));
    }
}
