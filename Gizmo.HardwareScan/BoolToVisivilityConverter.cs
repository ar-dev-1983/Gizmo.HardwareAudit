﻿using System;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.HardwareScan
{
    internal class BoolToVisivilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => value != null ? (bool)value == true ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotSupportedException();
        }
    }
}
