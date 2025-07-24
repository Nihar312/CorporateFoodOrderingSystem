using System.Globalization;

namespace FoodOrderingUI.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var param = parameter?.ToString()?.Split('|');
            return value is bool b ? param[0] : param[1];
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var param = parameter?.ToString()?.Split('|');
            return value is bool b ? param[0] : param[1];
        }
    }
}
