using System;
using System.Globalization;
using System.Windows.Data;

namespace Gizmo.HardwareAudit
{

    internal class TreeItemScanDateTimeToTextConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            string result;
            if (o is DateTime time)
            {
                if (time != new DateTime())
                {
                    result = time.ToLongDateString() + " " + time.ToLongTimeString();
                }
                else
                {
                    result = string.Empty;
                }
            }
            else
                result = string.Empty;

            return result;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
