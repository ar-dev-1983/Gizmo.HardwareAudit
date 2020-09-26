using System;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.HardwareAudit
{
    internal class BoolToFontStyle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => (bool)value ? FontStyles.Italic : FontStyles.Normal;
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotSupportedException();
        }
    }
}
