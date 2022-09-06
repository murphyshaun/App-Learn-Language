using System;
using System.Globalization;
using System.Windows.Data;

namespace LearnNewWord.Managers.Converter
{
    public class FormatCurrencyConverterCustom : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string text = value.ToString();
                if (text.Length > 0)
                {
                    value = decimal.Parse(text);
                }
            }
            return string.Format("{0:N2}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}