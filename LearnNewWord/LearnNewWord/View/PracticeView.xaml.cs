using LearnNewWord.IViewModel;
using LearnNewWord.Managers;
using System.Windows.Controls;

namespace LearnNewWord.View
{
    /// <summary>
    /// Interaction logic for PracticeView.xaml
    /// </summary>
    public partial class PracticeView : UserControl
    {
        public PracticeView()
        {
            InitializeComponent();
            DataContext = Manager.Resolve<IPracticeViewModel>();
        }
    }
}