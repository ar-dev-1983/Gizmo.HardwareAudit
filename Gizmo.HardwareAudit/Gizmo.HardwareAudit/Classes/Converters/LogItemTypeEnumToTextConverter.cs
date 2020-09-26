using Gizmo.HardwareAudit.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Gizmo.HardwareAudit
{
    internal class LogItemTypeEnumToTextConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value switch
                {
                    LogItemTypeEnum.Error => "E",
                    LogItemTypeEnum.Information => "I",
                    LogItemTypeEnum.Warning => "W",
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
