using AutoMapper;
using Common.Constants;
using Common.Dto;
using Common.Extension;
using LearnNewWord.IViewModel;
using LearnNewWord.Managers.Dialog;
using LearnNewWord.Managers.Enum;
using LearnNewWord.Managers.EventAgrregator;
using LearnNewWord.Managers.EventAgrregator.Payload;
using LearnNewWord.Managers.Extension;
using LearnNewWord.Model;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Events;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Imaging;

namespace LearnNewWord.ViewModel
{
    public class PracticeViewModel : BaseViewModel, IPracticeViewModel
    {
        #region Fields

        private BitmapImage _imageSuggestion;
        private WordModel _word;
        private bool _isPractice = false;
        private string _pathImage;
        private bool _isLearnEnglish = true;
        private string _message;
        private StatusType _status;
        private SettingPayload _settingPayload;
        private string _toolTipButtonAddUpdateWord = MessageManager.TOOL_TIP_ADD_WORD;
        private string _toolTipButtonPractice = MessageManager.TOOL_TIP_MODE_CRUD;
        private int _indexWordBeingPractice = -1;
        private readonly Timer _timer = new Timer(2000);
        private Random _random = new Random();

        #endregion Fields

        #region Properties

        public string ToolTipButtonAddUpdateWord
        {
            get => _toolTipButtonAddUpdateWord;
            set => SetProperty(ref _toolTipButtonAddUpdateWord, value);
        }

        public string ToolTipButtonPractice
        {
            get => _toolTipButtonPractice;
            set => SetProperty(ref _toolTipButtonPractice, value);
        }

        public BitmapImage ImageSuggestion
        {
            get => _imageSuggestion;
            set => SetProperty(ref _imageSuggestion, value);
        }

        public WordModel Word
        {
            get => _word;
            set => SetProperty(ref _word, value);
        }

        public bool IsLearnEnglish
        {
            get => _isLearnEnglish;
            set
            {
                SetProperty(ref _isLearnEnglish, value);

                UpdateWordPracticeEnglishVietNamese();
            }
        }

        public bool IsPractice
        {
            get => _isPractice;
            set => SetProperty(ref _isPractice, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public StatusType Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        #endregion Properties

        #region Commands

        public DelegateCommand AddUpdateNewWordCommand { get; set; }

        public DelegateCommand UploadImageCommand { get; set; }

        public DelegateCommand SwitchModeCommand { get; set; }

        public DelegateCommand ClearWordCommand { get; set; }

        public DelegateCommand KeyEnterPressCommand { get; set; }

        public DelegateCommand EnglishLostFocusCommand { get; set; }

        public DelegateCommand SoundPronounceCommand { get; set; }

        #endregion Commands

        #region Constructor

        public PracticeViewModel(IWordService wordService, IMapper mapper, IEventAggregator eventAggregator) : base(wordService, mapper, eventAggregator)
        {
            ImageSuggestion = LoadImageSuggestion(_imageDefault);

            AddUpdateNewWordCommand = new DelegateCommand(async () => await AddUpdateNewWordOnClick());

            SoundPronounceCommand = new DelegateCommand(async () => await SoundPronounceOnClick());

            UploadImageCommand = new DelegateCommand(UploadImageOnClick);

            SwitchModeCommand = new DelegateCommand(SwitchModeOnClick);

            ClearWordCommand = new DelegateCommand(ClearWordOnClick);

            KeyEnterPressCommand = new DelegateCommand(KeyEnterPress);

            EnglishLostFocusCommand = new DelegateCommand(async () => await EnglishLostFocusEvent());

            _eventAggregator.GetEvent<RefreshListWordEvent>().Subscribe(RefreshListWord);
            _eventAggregator.GetEvent<UpdateWordEvent>().Subscribe(BindingDataWordUpdate);

            Word = new WordModel();

            Status = StatusType.None;

            _timer.Elapsed += async (sender, e) => await RemoveNotification();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Adds the update new word on click.
        /// </summary>
        private async Task AddUpdateNewWordOnClick()
        {
            //check isexist id => true ? edit : add
            //var word = ListWord.FirstOrDefault(c => c.Id == Word.Id);

            var listItem = ValidateWord();
            if (listItem.Count > 0)
            {
                var message = string.Format($"Please enter {string.Join(", ", listItem)}");

                _ = MessageBoxCustom.ShowDialog("Attention", message, DialogButtonShow.OK, 250, 550);
            }
            else
            {
                if (!string.IsNullOrEmpty(_pathImage))
                    Word.Image = FileExtension.SaveFile(EnvironmentPath.PathImageStore, _pathImage);

                Message = Word.Id != default(int) ? await UpdateWord() : await AddWord();

                _timer.Start();
            }
        }

        /// <summary>
        /// Adds the word.
        /// </summary>
        /// <returns></returns>
        public async Task<string> AddWord()
        {
            var message = string.Empty;

            var word = _mapper.Map<WordDto>(Word);

            word.CategoryId = _settingPayload.CategoryId;

            var result = await _wordService.AddWord(word);

            if (result != 0)
            {
                message = MessageManager.ADD_WORD_SUCCESSFULL;
                Word = new WordModel();
                _pathImage = string.Empty;
                Status = StatusType.Success;

                ImageSuggestion = LoadImageSuggestion(_imageDefault);
            }
            else
            {
                message = MessageManager.FAIL;
                Status = StatusType.Fail;
            }

            return message;
        }

        /// <summary>
        /// Updates the word.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateWord()
        {
            var word = _mapper.Map<WordDto>(Word);

            word.CategoryId = _settingPayload.CategoryId;

            var result = await _wordService.UpdateWord(word);

            var message = string.Empty;

            if (result != 0)
            {
                message = MessageManager.UPDATE_WORD_SUCCESSFULL;
                Word = new WordModel();
                _pathImage = string.Empty;
                Status = StatusType.Success;

                ImageSuggestion = LoadImageSuggestion(_imageDefault);
            }
            else
            {
                message = MessageManager.FAIL;
                Status = StatusType.Fail;
            }

            return message;
        }

        /// <summary>
        /// Uploads the image on click.
        /// </summary>
        private void UploadImageOnClick()
        {
            _pathImage = FileExtension.FileDialog(FileUploadFilter.Image);

            if (string.IsNullOrEmpty(_pathImage)) return;

            var uriImage = new Uri(_pathImage);

            ImageSuggestion = new BitmapImage(uriImage);
        }

        /// <summary>
        /// Switches the mode on click.
        /// </summary>
        private void SwitchModeOnClick()
        {
            Status = StatusType.None;
            Message = string.Empty;

            if (IsPractice)
            {
                if (_settingPayload == null || _settingPayload.ListWordPractice.Count() == 0)
                {
                    IsPractice = false;
                    return;
                }

                GetWordPractice();
                ToolTipButtonPractice = MessageManager.TOOL_TIP_MODE_PRACTICE;
            }
            else
            {
                ToolTipButtonPractice = MessageManager.TOOL_TIP_MODE_CRUD;
                ToolTipButtonAddUpdateWord = Word.Id != default(int) ? MessageManager.TOOL_TIP_UPDATE_WORD : MessageManager.TOOL_TIP_MODE_PRACTICE;

                if (string.IsNullOrEmpty(Word?.English))
                {
                    ImageSuggestion = LoadImageSuggestion(_imageDefault);
                }
            }
        }

        /// <summary>
        /// Refreshes the list word.
        /// </summary>
        private void RefreshListWord(SettingPayload settingPayload)
        {
            _settingPayload = settingPayload;
            _indexWordBeingPractice = -1;
            if (IsPractice)
                GetWordPractice();
        }

        /// <summary>
        /// Bindings the data word update.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void BindingDataWordUpdate(WordModel payload)
        {
            IsPractice = false;

            Word = payload;

            ImageSuggestion = LoadImageSuggestion(payload.Image);

            ToolTipButtonAddUpdateWord = MessageManager.TOOL_TIP_UPDATE_WORD;
        }

        /// <summary>
        /// Gets the word practice.
        /// </summary>
        /// <param name="index">The index.</param>
        private void GetWordPractice()
        {
            if (_settingPayload.IsRandom)
            {
                _indexWordBeingPractice = _random.Next(_settingPayload.FromIndex, _settingPayload.ToIndex + 1) - 1;
            }
            else
            {
                if (_indexWordBeingPractice == (_settingPayload.ListWordPractice.Count() - 1))
                {
                    _indexWordBeingPractice = -1;
                }

                _indexWordBeingPractice++;
            }

            Word = (WordModel)_settingPayload.ListWordPractice.ElementAt(_indexWordBeingPractice).Clone();

            ImageSuggestion = LoadImageSuggestion(Word.Image);

            if (IsLearnEnglish)
            {
                Word.Vietnamese = string.Empty;
            }
            else
            {
                Word.English = string.Empty;
            }
        }

        /// <summary>
        /// Clears the word on click.
        /// </summary>
        /// <returns></returns>
        private void ClearWordOnClick()
        {
            var result = MessageBoxCustom.ShowDialog("Attention", "Are you sure want to clear word?", DialogButtonShow.YesNo, 250, 400);

            if (result == DialogResult.Yes)
            {
                Word = new WordModel();
                ImageSuggestion = LoadImageSuggestion(_imageDefault);
                ToolTipButtonAddUpdateWord = MessageManager.TOOL_TIP_MODE_PRACTICE;
            }
        }

        /// <summary>
        /// Validates the word.
        /// </summary>
        /// <returns></returns>
        private List<string> ValidateWord()
        {
            var listItem = new List<string>();

            if (string.IsNullOrEmpty(Word.English))
            {
                listItem.Add("English");
            }

            if (string.IsNullOrEmpty(Word.IpaText))
            {
                listItem.Add("IpaText");
            }

            if (string.IsNullOrEmpty(Word.Vietnamese))
            {
                listItem.Add("Vietnamese");
            }

            if (string.IsNullOrEmpty(Word.Example))
            {
                listItem.Add("Example");
            }

            return listItem;
        }

        /// <summary>
        /// Keys the enter press.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void KeyEnterPress()
        {
            if (IsPractice)
            {
                //mode practice => check word
                var wordCompare = _settingPayload.ListWordPractice.FirstOrDefault(c => c.Id == Word.Id);
                if (wordCompare != null)
                {
                    var isSame = false;
                    if (IsLearnEnglish)
                    {
                        isSame = StringExtension.IsEquivalentTo(wordCompare?.Vietnamese?.ToLower(), Word?.Vietnamese?.ToLower());
                    }
                    else
                    {
                        isSame = StringExtension.IsEquivalentTo(wordCompare?.English?.ToLower(), Word?.English?.ToLower());
                    }

                    if (isSame)
                    {
                        Message = MessageManager.WORD_RIGHT;
                        Status = StatusType.Success;

                        GetWordPractice();
                    }
                    else
                    {
                        Message = MessageManager.WORD_WRONG;
                        Status = StatusType.Fail;
                    }

                    _timer.Start();
                }
            }
            else
            {
                //mode crud => check add or update
                Task.WhenAll(AddUpdateNewWordOnClick());
            }
        }

        /// <summary>
        /// Updates the word english viet namese.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void UpdateWordPracticeEnglishVietNamese()
        {
            var wordBeingPractice = _settingPayload?.ListWordPractice?.FirstOrDefault(c => c.Id == Word.Id);

            if (wordBeingPractice != null)
            {
                if (IsLearnEnglish)
                {
                    Word.English = wordBeingPractice.English;
                    Word.Vietnamese = string.Empty;
                }
                else
                {
                    Word.Vietnamese = wordBeingPractice.Vietnamese;
                    Word.English = string.Empty;
                }
            }
        }

        /// <summary>
        /// Removes the notification.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private async Task RemoveNotification()
        {
            Message = string.Empty;
            Status = StatusType.None;
            _timer.Stop();
        }

        /// <summary>
        /// Englishes the lost focus event.
        /// </summary>
        private async Task EnglishLostFocusEvent()
        {
            if (!IsPractice && !string.IsNullOrEmpty(Word.English))
            {
                var item = _settingPayload.AllWord?.FirstOrDefault(c => c.English.ToLower().IsEquivalentTo(Word.English?.ToLower()));

                Status = item != null ? StatusType.Fail : StatusType.None;

                if (item != null)
                {
                    Status = StatusType.Warning;
                    Message = MessageManager.WORD_EXIST;
                    _timer.Start();
                }
                else
                {
                    await BindingWordInfo();
                }
            }
        }

        /// <summary>
        /// Bindings the word ipa into textbox.
        /// </summary>
        private async Task BindingWordInfo()
        {
            try
            {
                var result = await CallOxforddictionariesApi(Word.English);
                Word.IpaText = result["results"][0]["lexicalEntries"][0]["entries"][0]["pronunciations"][1]["phoneticSpelling"].ToString();

                var categoryWord = result["results"][0]["lexicalEntries"][0]["lexicalCategory"]["id"].ToString().ToLower();

                dataAudioFromApi[Word.English] = result["results"][0]["lexicalEntries"][0]["entries"][0]["pronunciations"][1]["audioFile"].ToString();
                Word.IsNoun = Word.IsVerb = Word.IsAjective = Word.IsAdverb = false;
                switch (categoryWord)
                {
                    case "noun":
                        {
                            Word.IsNoun = true;
                            break;
                        }

                    case "verb":
                        {
                            Word.IsVerb = true;
                            break;
                        }

                    case "adjective":
                        {
                            Word.IsAjective = true;
                            break;
                        }

                    case "adverb":
                        {
                            Word.IsAdverb = true;
                            break;
                        }
                }

            }
            catch (Exception)
            {
                Message = MessageManager.WORD_WRONG;
                Status = StatusType.Fail;
                _timer.Start();
            }
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
            synthesizer.Speak(Word.English);
        }

        /// <summary>
        /// Plays from HTTP request.
        /// </summary>
        private async Task PlayFromHttpRequest()
        {
            var wplayer = new WMPLib.WindowsMediaPlayer();

            if (dataAudioFromApi.ContainsKey(Word.English))
            {
                wplayer.URL = dataAudioFromApi[Word.English];
            }
            else
            {
                var result = await CallOxforddictionariesApi(Word.English);
                wplayer.URL = result["results"][0]["lexicalEntries"][0]["entries"][0]["pronunciations"][1]["audioFile"].ToString();
                dataAudioFromApi[Word.English] = wplayer.URL;
            }

            wplayer.controls.play();
        }
        #endregion Methods
    }
}