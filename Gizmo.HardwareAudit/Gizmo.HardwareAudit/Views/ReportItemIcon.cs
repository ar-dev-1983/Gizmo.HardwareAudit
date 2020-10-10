using Gizmo.HardwareAudit.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.HardwareAudit.Views
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
        public static readonly DependencyProperty ItemTypeProperty = DependencyProperty.Register("ItemType", typeof(ReportItemTypeEnum), typeof(ReportItemIcon), new UIPropertyMetadata(ReportItemTypeEnum.None));
    }
}
