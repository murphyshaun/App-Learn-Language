using LearnNewWord.IViewModel;
using LearnNewWord.Managers;
using System.Windows;

namespace LearnNewWord.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = Manager.Resolve<IBaseViewModel>();
        }
    }
}