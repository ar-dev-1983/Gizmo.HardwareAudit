using Gizmo.HardwareAudit.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.HardwareAudit
{
    public class DomainDiscoveryToIconConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value switch
                {
                    DomainDiscoveryModeEnum.FromDomainController => "Domain Cotroller",
                    DomainDiscoveryModeEnum.FromDomainName => "Domain",
                    _ => string.Empty
                };
            }
            else
            {
                return string.Empty;
            }

            //StackPanel result = new StackPanel() { Margin = new Thickness(3), Orientation = Orientation.Horizontal };
            //if (value is DomainDiscoveryModeEnum enumValue)
            //{
            //    if (enumValue == DomainDiscoveryModeEnum.FromDomainController)
            //    {
            //        result.Children.Add(new GizmoIcon() { FontSize = 16, Icon = GizmoComputerHardwareIconsEnum.Container2, Margin = new Thickness(2, 0, 2, 0), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
            //        result.Children.Add(new TextBlock() { Text = "Domain Controller", Margin = new Thickness(2,0,2,0), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Width = 120 });
            //    }
            //    else if (enumValue == DomainDiscoveryModeEnum.FromDomainName)
            //    {
            //        result.Children.Add(new GizmoIcon() { FontSize = 16, Icon = GizmoComputerHardwareIconsEnum.ActiveDirectory, Margin = new Thickness(2, 0, 2, 0), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
            //        result.Children.Add(new TextBlock() { Text = "Domain", Margin = new Thickness(2, 0, 2, 0), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Width = 120 });
            //    }
            //}
            //return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
