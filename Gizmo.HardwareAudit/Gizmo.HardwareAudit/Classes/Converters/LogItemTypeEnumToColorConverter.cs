using Gizmo.HardwareAudit.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Gizmo.HardwareAudit
{
    internal class LogItemTypeEnumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value switch
                {
                    LogItemTypeEnum.Error => Brushes.Red,
                    LogItemTypeEnum.Information => Brushes.Green,
                    LogItemTypeEnum.Warning => Brushes.Orange,
                    _ => Brushes.Transparent
                };
            }
            else
            {
                return Brushes.Transparent;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
