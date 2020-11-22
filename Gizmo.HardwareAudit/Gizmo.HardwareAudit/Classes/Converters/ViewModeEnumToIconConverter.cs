using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditWPF;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.HardwareAudit
{
    public class ViewModeEnumToIconConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value switch
                {
                    UIViewModeEnum.All => GizmoComputerHardwareIconsEnum.ComputerAll,
                    UIViewModeEnum.SystemEnclosure => GizmoComputerHardwareIconsEnum.SystemEnclosure,
                    UIViewModeEnum.CPUs => GizmoComputerHardwareIconsEnum.CPU,
                    UIViewModeEnum.MemoryDevices => GizmoComputerHardwareIconsEnum.MemoryDevice,
                    UIViewModeEnum.NetworkAdapters => GizmoComputerHardwareIconsEnum.NetworkAdapter,
                    UIViewModeEnum.VideoControllers => GizmoComputerHardwareIconsEnum.VideoAdapter,
                    UIViewModeEnum.PhysicalDrives => GizmoComputerHardwareIconsEnum.PhysicalDisk,
                    UIViewModeEnum.Partitions => GizmoComputerHardwareIconsEnum.Partition,
                    UIViewModeEnum.Printers => GizmoComputerHardwareIconsEnum.Printer,
                    UIViewModeEnum.Licenses => GizmoComputerHardwareIconsEnum.Windows,
                    UIViewModeEnum.Displays => GizmoComputerHardwareIconsEnum.Monitor,
                    UIViewModeEnum.LocalUsers => GizmoComputerHardwareIconsEnum.Users,
                    UIViewModeEnum.LocalGroups => GizmoComputerHardwareIconsEnum.UserGroup,
                    _ => GizmoComputerHardwareIconsEnum.None
                };
            }
            else
            {
                return GizmoComputerHardwareIconsEnum.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
