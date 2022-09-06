using AutoMapper;
using LearnNewWord.IViewModel;
using LearnNewWord.Managers.Dialog;
using LearnNewWord.Managers.Enum;
using LearnNewWord.Managers.EventAgrregator;
using LearnNewWord.Managers.EventAgrregator.Payload;
using LearnNewWord.Model;
using Prism.Commands;
using Prism.Events;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LearnNewWord.ViewModel
{
    public class UtilitiesViewModel : BaseViewModel, IUtilitiesViewModel
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private CategoryModel _categorySelected;
        private ObservableCollection<CategoryModel> _listCategory = new ObservableCollection<CategoryModel>();
        private List<WordModel> _listWord = new List<WordModel>();
        private WordModel _wordSelected;
        private int _totalRow;
        private WordModel _flashCardWord;
        private BitmapImage _imageFlashCard;
        private int _currentIndex;
        private bool _isUseImage = true;
        private bool _isGetAllSetting = true;
        private int _fromIndex;
        private int _toIndex;
        private string _nameCategory;
        private bool _isRandom;
        private bool _isKnownSetting;
        private ObservableCollection<WordModel> _listWordFilter;
        private string _textSearch = string.Empty;
        #endregion Fields

        #region Properties

        public CategoryModel CategorySelected
        {
            get => _categorySelected;
            set
            {
                SetProperty(ref _categorySelected, value);
                Task.WhenAll(GetWords());
            }
        }

        public ObservableCollection<CategoryModel> ListCategory
        {
            get => _listCategory;
            set => SetProperty(ref _listCategory, value);
        }

        public WordModel WordSelected
        {
            get => _wordSelected;
            set => SetProperty(ref _wordSelected, value);
        }

        public ObservableCollection<WordModel> ListWordFilter
        {
            get => _listWordFilter;
            set => SetProperty(ref _listWordFilter, value);
        }

        public int TotalRow
        {
            get => _totalRow;
            set => SetProperty(ref _totalRow, value);
        }

        public WordModel FlashCardWord
        {
            get => _flashCardWord;
            set => SetProperty(ref _flashCardWord, value);
        }

        public BitmapImage ImageFlashCard
        {
            get => _imageFlashCard;
            set => SetProperty(ref _imageFlashCard, value);
        }

        public bool IsUseImage
        {
            get => _isUseImage;
            set => SetProperty(ref _isUseImage, value);
        }

        public bool IsGetAllSetting
        {
            get => _isGetAllSetting;
            set
            {
                SetProperty(ref _isGetAllSetting, value);
                ToIndex = value ? _listWord.Count : _listWord.Count(c => c.IsKnown == _isKnownSetting);
            }
        }

        public bool IsKnownSetting
        {
            get => _isKnownSetting;
            set
            {
                SetProperty(ref _isKnownSetting, value);
                ToIndex = _listWord.Count(c => c.IsKnown == value);
                FromIndex = ToIndex > 0 ? 1 : 0;
            }
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

        public string NameCategory
        {
            get => _nameCategory;
            set => SetProperty(ref _nameCategory, value);
        }

        public bool IsRandom
        {
            get => _isRandom;
            set => SetProperty(ref _isRandom, value);
        }

        public string TextSearch
        {
            get => _textSearch;
            set => SetProperty(ref _textSearch, value);
        }
        #endregion Properties

        #region Commands

        public DelegateCommand PreviousItemCommand { get; set; }

        public DelegateCommand NextItemCommand { get; set; }

        public DelegateCommand SoundPronounceCommand { get; set; }

        public DelegateCommand<WordModel> EditItemCommand { get; set; }

        public DelegateCommand<WordModel> RemoveItemCommand { get; set; }

        public DelegateCommand SearchNewWordCommand { get; set; }

        public DelegateCommand RefreshWordsCommand { get; set; }

        public DelegateCommand AddCategoryCommand { get; set; }

        public DelegateCommand RefreshCategoryCommand { get; set; }

        #endregion Commands

        #region Constructor

        public UtilitiesViewModel(ICategoryService categoryService, IMapper mapper, IWordService wordService, IEventAggregator eventAggregator) : base(wordService, mapper, eventAggregator)
        {
            _categoryService = categoryService;

            PreviousItemCommand = new DelegateCommand(PreviousItemOnClick);

            NextItemCommand = new DelegateCommand(NextItemOnClick);

            SoundPronounceCommand = new DelegateCommand(async () => await SoundPronounceOnClick());

            EditItemCommand = new DelegateCommand<WordModel>(EditItemOnClick);

            RemoveItemCommand = new DelegateCommand<WordModel>(async (word) => await RemoveItemOnClick(word));

            SearchNewWordCommand = new DelegateCommand(SearchNewWordOnClick);

            RefreshWordsCommand = new DelegateCommand(RefreshWordsOnClick);

            AddCategoryCommand = new DelegateCommand(async () => await AddCategoryOnClick());

            RefreshCategoryCommand = new DelegateCommand(async () => await RefreshCategoryOnClick());

            Task.WhenAll(GetCategory());
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets the category.
        /// </summary>
        private async Task GetCategory()
        {
            var result = await _categoryService.GetListCategory();

            ListCategory.Clear();

            ListCategory.AddRange(new ObservableCollection<CategoryModel>(_mapper.Map<IEnumerable<CategoryModel>>(result)));

            if (ListCategory.Count > 0)
            {
                CategorySelected = ListCategory[0];
            }
        }

        /// <summary>
        /// Gets the words.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private async Task GetWords()
        {
            if (CategorySelected == null) return;

            var words = await _wordService.GetListWord(CategorySelected.Id);

            _listWord.Clear();

            _listWord.AddRange(new ObservableCollection<WordModel>(_mapper.Map<IEnumerable<WordModel>>(words)));

            ListWordFilter = new ObservableCollection<WordModel>(_listWord.Where(c => c.English.ToLower().Contains(TextSearch) || c.Vietnamese.ToLower().Contains(TextSearch)));

            TotalRow = _listWord.Count;

            _currentIndex = 0;

            LoadDataFlashCard();

            ToIndex = TotalRow;
            FromIndex = TotalRow > 0 ? 1 : 0;
            IsGetAllSetting = true;

            _eventAggregator.GetEvent<RefreshListWordEvent>().Publish(
                new SettingPayload()
                {
                    FromIndex = FromIndex,
                    ToIndex = ToIndex,
                    CategoryId = CategorySelected.Id,
                    ListWordPractice = _listWord,
                    AllWord = _listWord,
                    IsRandom = IsRandom
                });
        }

        /// <summary>
        /// Loads the data flash card.
        /// </summary>
        private void LoadDataFlashCard()
        {
            if (_listWord == null || _listWord.Count == 0)
            {
                FlashCardWord = null;
                ImageFlashCard = LoadImageSuggestion(_imageDefault);
                return;
            }

            FlashCardWord = _listWord[_currentIndex];
            ImageFlashCard = LoadImageSuggestion(FlashCardWord.Image);
            IsUseImage = true;
        }

        /// <summary>
        /// Nexts the item on click.
        /// </summary>
        private void NextItemOnClick()
        {
            if (TotalRow != 0 && _currentIndex == TotalRow - 1) _currentIndex = -1;

            _currentIndex++;
            LoadDataFlashCard();
        }

        /// <summary>
        /// Previouses the item on click.
        /// </summary>
        private void PreviousItemOnClick()
        {
            if (_currentIndex == 0) _currentIndex = TotalRow;

            _currentIndex--;
            LoadDataFlashCard();
        }

        /// <summary>
        /// Sounds the pronounce on click.
        /// </summary>
        private async Task SoundPronounceOnClick()
        {
            try
            {
                await PlayFromHttpRequest();
            }
            catch (Exception)
            {
                PlayFromWindow();
            }
        }

        /// <summary>
        /// Plays from window.
        /// </summary>
        private void PlayFromWindow()
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak(FlashCardWord.English);
        }

        /// <summary>
        /// Plays from HTTP request.
        /// </summary>
        private async Task PlayFromHttpRequest()
        {
            var wplayer = new WMPLib.WindowsMediaPlayer();

            if (dataAudioFromApi.ContainsKey(FlashCardWord.English))
            {
                wplayer.URL = dataAudioFromApi[FlashCardWord.English];
            }
            else
            {
                var result = await CallOxforddictionariesApi(FlashCardWord.English);
                wplayer.URL = result["results"][0]["lexicalEntries"][0]["entries"][0]["pronunciations"][1]["audioFile"].ToString();
                dataAudioFromApi[FlashCardWord.English] = wplayer.URL;
            }

            wplayer.controls.play();
        }

        /// <summary>
        /// Refreshes the words on click.
        /// </summary>
        private void RefreshWordsOnClick()
        {
            var listWordPractice = new List<WordModel>(_listWord);

            if (!IsGetAllSetting)
            {
                listWordPractice = listWordPractice.Where(c => c.IsKnown == IsKnownSetting).ToList();

                listWordPractice = listWordPractice.GetRange(FromIndex - 1, ToIndex - FromIndex + 1);
            }

            _eventAggregator.GetEvent<RefreshListWordEvent>().Publish(
                new SettingPayload()
                {
                    FromIndex = FromIndex,
                    ToIndex = ToIndex,
                    CategoryId = CategorySelected.Id,
                    ListWordPractice = listWordPractice,
                    AllWord = _listWord,
                    IsRandom = IsRandom
                });
            MessageBox.Show("Refresh successful", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Searches the new word on click.
        /// </summary>
        private void SearchNewWordOnClick()
        {
            ListWordFilter = new ObservableCollection<WordModel>(_listWord.Where(c => c.English.ToLower().Contains(TextSearch) || c.Vietnamese.ToLower().Contains(TextSearch)));
        }

        /// <summary>
        /// Removes the item on click.
        /// </summary>
        /// <param name="word">The word.</param>
        private async Task RemoveItemOnClick(WordModel word)
        {
            var result = MessageBoxCustom.ShowDialog("Attention", "Are you sure want to delete item?", DialogButtonShow.DeleteCancel, 250, 400);

            if (result == DialogResult.Delete)
            {
                var item = _listWord.FirstOrDefault(c => c.Id == word?.Id);

                if (item != null)
                {
                    var resultChangeData = await _wordService.DeleteWord(item.Id);

                    if (resultChangeData != 0)
                    {
                        await RefreshCategoryOnClick();
                    }
                }
            }
        }

        /// <summary>
        /// Edits the item on click.
        /// </summary>
        /// <param name="word">The word.</param>
        private void EditItemOnClick(WordModel word)
        {
            _eventAggregator.GetEvent<UpdateWordEvent>().Publish(word);
        }

        /// <summary>
        /// Adds the category on click.
        /// </summary>
        /// <returns></returns>
        private async Task AddCategoryOnClick()
        {
            if (string.IsNullOrEmpty(NameCategory)) return;

            var result = await _categoryService.AddCategory(NameCategory);

            if (result != 0)
            {
                NameCategory = string.Empty;
                await GetCategory();
            }
            else
            {
                MessageBox.Show("Error", "Attention", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Refreshes the category on click.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshCategoryOnClick()
        {
            await GetCategory();
            MessageBox.Show("Refresh successful", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion Methods
    }
}