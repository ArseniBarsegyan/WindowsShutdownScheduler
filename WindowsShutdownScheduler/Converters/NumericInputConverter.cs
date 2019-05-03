using System;
using System.Globalization;
using System.Windows.Data;

namespace WindowsShutdownScheduler.Converters
{
    public class NumericInputConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return int.TryParse(value.ToString(), out var number);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}