using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace STranslate.Plugin.Tts.Vocu.Converter;

/// <summary>
/// 字符串为空时返回 Collapsed，否则返回 Visible
/// </summary>
public sealed class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
