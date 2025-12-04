using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using FalloutVault.AvaloniaApp.ViewModels;

namespace FalloutVault.AvaloniaApp.Converters;

public class LogDataGridRowConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is LogViewModel logVm)
        {
            return logVm.IsAlert
                ? Brushes.Red
                : Brushes.White;
        }

        return value;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}