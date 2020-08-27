using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FuzzyExpert.WpfClient.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool.TryParse(parameter?.ToString(), out var reversed);
            if (reversed)
            {
                return !(value is bool booleanVisibility) ?
                    Visibility.Hidden :
                    booleanVisibility ? Visibility.Hidden : Visibility.Visible;
            }
            else
            {
                return !(value is bool booleanVisibility) ?
                    Visibility.Hidden :
                    booleanVisibility ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}