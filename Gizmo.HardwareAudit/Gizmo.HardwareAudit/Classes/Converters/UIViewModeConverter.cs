using Gizmo.HardwareAuditWPF;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.HardwareAudit
{
    public class UIViewModeConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value switch
                {
                    UIViewModeEnum.All => Visibility.Visible,
                    UIViewModeEnum.CPUs when parameter.ToString() == "CPUs" => Visibility.Visible,
                    UIViewModeEnum.Displays when parameter.ToString() == "Displays" => Visibility.Visible,
                    UIViewModeEnum.Licenses when parameter.ToString() == "Licenses" => Visibility.Visible,
                    UIViewModeEnum.MemoryDevices when parameter.ToString() == "MemoryDevices" => Visibility.Visible,
                    UIViewModeEnum.NetworkAdapters when parameter.ToString() == "NetworkAdapters" => Visibility.Visible,
                    UIViewModeEnum.Partitions when parameter.ToString() == "Partitions" => Visibility.Visible,
                    UIViewModeEnum.PhysicalDrives when parameter.ToString() == "PhysicalDrives" => Visibility.Visible,
                    UIViewModeEnum.Printers when parameter.ToString() == "Printers" => Visibility.Visible,
                    UIViewModeEnum.SystemEnclosure when parameter.ToString() == "SystemEnclosure" => Visibility.Visible,
                    UIViewModeEnum.VideoControllers when parameter.ToString() == "VideoControllers" => Visibility.Visible,
                    _ => Visibility.Collapsed
                };
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
