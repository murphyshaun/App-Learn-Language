using LearnNewWord.IViewModel;
using LearnNewWord.Managers;
using System.Windows.Controls;

namespace LearnNewWord.View
{
    /// <summary>
    /// Interaction logic for UtilitiesView.xaml
    /// </summary>
    public partial class UtilitiesView : UserControl
    {
        public UtilitiesView()
        {
            InitializeComponent();
            DataContext = Manager.Resolve<IUtilitiesViewModel>();
        }
    }
}