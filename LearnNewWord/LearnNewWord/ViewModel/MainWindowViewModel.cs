using AutoMapper;
using LearnNewWord.IViewModel;
using LearnNewWord.Model;
using Prism.Commands;
using Prism.Events;
using Service.IService;
using System.Windows.Media.Imaging;

namespace LearnNewWord.ViewModel
{
    public class MainWindowViewModel : BaseViewModel, IMainWindowViewModel
    {
        #region Fields

        private int _totalRow;

        private WordModel _selectedNewWord;




        private int _fromIndex;
        private int _toIndex;
        private bool _isGetAllSetting;

        private int _currentIndexPractice;

        #endregion Fields

        #region Properties



        public int TotalRow
        {
            get => _totalRow;
            set => SetProperty(ref _totalRow, value);
        }

        public WordModel SelectedNewWord
        {
            get => _selectedNewWord;
            set => SetProperty(ref _selectedNewWord, value);
        }




        public int FromIndex
        {
            get => _fromIndex;
            set => SetProperty(ref _fromIndex, value);
        }

        public int ToIndex
        {
            get => _toIndex;
            set => SetProperty(ref _toIndex, value);
        }

        public bool IsGetAllSetting
        {
            get => _isGetAllSetting;
            set => SetProperty(ref _isGetAllSetting, value);
        }

        #endregion Properties

        #region Command


        public DelegateCommand SwitchModeCommand { get; set; }
        public DelegateCommand SearchNewWordCommand { get; set; }

        public DelegateCommand RefreshWordsCommand { get; set; }
        public DelegateCommand CheckWordCommand { get; set; }

        #endregion Command

        #region Constructor

        public MainWindowViewModel(IMapper mapper, IWordService wordService, IEventAggregator eventAggregator) : base(wordService, mapper, eventAggregator)
        {
            InitalData();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Initals the data.
        /// </summary>
        private void InitalData()
        {
            CheckWordCommand = new DelegateCommand(CheckWordEnter);

            GetAllWordFromFile();

        }

        private void CheckWordEnter()
        {
            //if (IsPractice)
            //{
            //    IsEqualWord = IsPracticeEnglish ? _wordPractice.Vietnamese.Equals(NewWord.Vietnamese)
            //                                : _wordPractice.English.Equals(NewWord.English);

            //    if (IsEqualWord)
            //    {
            //        _currentIndexPractice++;

            //        if (_currentIndexPractice > ToIndex)
            //            _currentIndexPractice = FromIndex;

            //        GetWordPractice(_currentIndexPractice);
            //    }
            //}
        }

        private void RefreshWordsOnClick()
        {
            GetAllWordFromFile();

            //if (!IsGetAllSetting)
            //{
            //    if (FromIndex < ToIndex)
            //        NewWordModels = new ObservableCollection<NewWordModel>(NewWordModels.ToList().GetRange(FromIndex, ToIndex - FromIndex));

            //    NewWordModels = new ObservableCollection<NewWordModel>(NewWordModels.Where(c => c.IsKnew == IsKnownSetting));
            //}

            _currentIndexPractice = FromIndex;
            //GetWordPractice(_currentIndexPractice);
        }









        private void RemoveItemOnClick(WordModel word)
        {
            //NewWordModels.Remove(word);
            //var pathFile = Path.Combine(EnvironmentPath.PathImageStore, word.Image);
            //FileExtension.DeleteFile(pathFile);
        }

        private void GetAllWordFromFile()
        {
            //var lstData = FileExtension.ReadFile<NewWordModel>(EnvironmentPath.PathJsonData);

            //NewWordModels = new ObservableCollection<NewWordModel>(lstData);

            //TotalRow = NewWordModels.Count;
            //ToIndex = TotalRow - 1;
            //IsGetAllSetting = true;
        }

        /// <summary>
        /// Mouses the double click word on click.
        /// </summary>
        private void EditItemOnClick(WordModel word)
        {
            //IsPractice = false;
            //NewWord = word;
            //ImageSuggestion = LoadImageSuggestion(NewWord.Image);
        }



        #endregion Methods
    }
}