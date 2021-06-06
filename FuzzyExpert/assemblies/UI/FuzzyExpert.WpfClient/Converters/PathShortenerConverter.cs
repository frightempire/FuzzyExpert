using System;
using System.Globalization;
using System.Windows.Data;

namespace FuzzyExpert.WpfClient.Converters
{
    class PathShortenerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value?.ToString();
            return string.IsNullOrEmpty(path) ? string.Empty : path.Substring(path.LastIndexOf('\\') + 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}