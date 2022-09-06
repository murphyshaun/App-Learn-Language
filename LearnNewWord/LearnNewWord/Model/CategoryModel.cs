using Prism.Mvvm;

namespace LearnNewWord.Model
{
    public class CategoryModel : BindableBase
    {
        #region Fields

        private int _id;
        private string _name;
        private int _total;

        #endregion Fields

        #region Properties

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        #endregion Properties
    }
}