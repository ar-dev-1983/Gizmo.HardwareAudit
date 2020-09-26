using Gizmo.HardwareAudit.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Gizmo.HardwareAudit
{
    internal class TreeItemStatusTextConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value switch
                {
                    ItemStatusEnum.Container => string.Empty,
                    ItemStatusEnum.Error => "Error",
                    ItemStatusEnum.Offline => "Offline",
                    ItemStatusEnum.Online => "Online",
                    ItemStatusEnum.OnlineAndCheckingTCPPorts => "Checking for open TCP Ports",
                    ItemStatusEnum.OnlineAndCheckingSharedFolders => "Searching for shared folders",
                    ItemStatusEnum.OnlineAndFetchingData => "Scanning hardware",
                    ItemStatusEnum.OnlineAndHasData => "Online and scan available",
                    ItemStatusEnum.OnlineButHasNoData => "Online, but no scan available",
                    ItemStatusEnum.Unknown => string.Empty,
                    ItemStatusEnum.OnlineAndDataHasChanged => "Online and detected hardware changes in scan",
                    _ => string.Empty
                };
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
