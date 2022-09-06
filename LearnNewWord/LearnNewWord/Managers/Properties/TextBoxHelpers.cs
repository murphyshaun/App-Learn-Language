using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LearnNewWord.Managers.Properties
{
    public static class TextBoxHelpers
    {
        #region IsNumberic

        public static bool GetIsNumeric(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsNumericProperty);
        }

        public static void SetIsNumeric(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsNumeric.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNumericProperty =
            DependencyProperty.RegisterAttached("IsNumeric", typeof(bool), typeof(TextBoxHelpers), new PropertyMetadata(false, OnValueChange));

        private static void OnValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.KeyUp += TextBoxChangeValue;
                textBox.LostFocus += TextBoxLostFocus;
            }
        }

        private static void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textbox)
            {
                textbox.Text = string.IsNullOrEmpty(textbox.Text) ? "0" : textbox.Text;
            }
        }

        private static void TextBoxChangeValue(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is TextBox textbox)
            {
                textbox.Text = Regex.Replace(textbox.Text, @"\D", string.Empty);
                textbox.Text = Regex.Replace(textbox.Text, @"^0+", string.Empty);
            }
        }

        #endregion IsNumberic
    }
}