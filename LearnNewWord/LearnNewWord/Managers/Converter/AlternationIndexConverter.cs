using System;
using System.Globalization;
using System.Windows.Data;

namespace LearnNewWord.Managers.Converter
{
    public class AlternationIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int result)
            {
                return result % 2;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}