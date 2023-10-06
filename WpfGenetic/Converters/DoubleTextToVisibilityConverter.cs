using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfGenetic.Converters;

public class DoubleTextToVisibilityConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string stringValue)
        {
            throw new ArgumentException($"Аргумент {value} должен быть bool");
        }

        return !(double.TryParse(stringValue.Replace('.', ','), out _) 
                                         && System.Convert.ToDouble(stringValue.Replace('.', ',')) > 0 
                                         && System.Convert.ToDouble(stringValue.Replace('.', ',')) < 1) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}