using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfGenetic.Converters;

public class IntegerTextToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string stringValue)
        {
            throw new ArgumentException($"Аргумент {value} должен быть bool");
        }

        return !(int.TryParse(stringValue.Replace('.', ','), out _)
                 && System.Convert.ToInt32(stringValue.Replace('.', ',')) > 0) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}