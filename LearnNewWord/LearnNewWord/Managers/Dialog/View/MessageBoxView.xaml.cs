using System.Windows;

namespace LearnNewWord.Managers.Dialog.View
{
    /// <summary>
    /// Interaction logic for MessageBoxView.xaml
    /// </summary>
    public partial class MessageBoxView : Window
    {
        public MessageBoxView()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}