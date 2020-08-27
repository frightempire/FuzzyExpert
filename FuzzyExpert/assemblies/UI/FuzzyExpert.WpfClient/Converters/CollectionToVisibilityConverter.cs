using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace FuzzyExpert.WpfClient.Converters
{
    public class CollectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool.TryParse(parameter?.ToString(), out var reversed);
            if (reversed)
            {
                return !(value is IEnumerable<object> collection) ?
                    Visibility.Hidden :
                    collection.ToList().Count == 0 ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                return !(value is IEnumerable<object> collection) ?
                    Visibility.Hidden :
                    collection.ToList().Count == 0 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}