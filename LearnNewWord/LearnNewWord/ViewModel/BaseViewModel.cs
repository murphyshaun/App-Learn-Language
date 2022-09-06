using AutoMapper;
using Common.Extension;
using LearnNewWord.IViewModel;
using LearnNewWord.Managers.Enum;
using Newtonsoft.Json.Linq;
using Prism.Events;
using Prism.Mvvm;
using Service.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LearnNewWord.ViewModel
{
    public class BaseViewModel : BindableBase, IBaseViewModel
    {
        #region Fields

        protected readonly string _imageDefault = "nodata.png";
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IMapper _mapper;
        protected readonly IWordService _wordService;
        private string _dateTimeFormat;
        private DispatcherTimer _timer;
        protected static Dictionary<string, string> dataAudioFromApi = new Dictionary<string, string>();

        #endregion Fields

        #region Properties

        public string DateTimeFormat
        {
            get => _dateTimeFormat;
            set => SetProperty(ref _dateTimeFormat, value);
        }

        #endregion Properties

        #region Constuctor

        public BaseViewModel(IWordService wordService, IMapper mapper, IEventAggregator eventAggregator)
        {
            _wordService = wordService;
            _mapper = mapper;
            _eventAggregator = eventAggregator;
            RunClock();
        }

        #endregion Constuctor

        #region Methods

        private void RunClock()
        {
            _timer = new DispatcherTimer();

            _timer.Tick += new EventHandler(Timer_Click);

            _timer.Interval = new TimeSpan(0, 0, 1);

            _timer.Start();
        }

        /// <summary>
        /// Handles the Click event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Timer_Click(object sender, EventArgs e)
        {
            var day = DateTime.Today.DayOfWeek;

            var dateTimeNow = DateTime.Now;
            var date = dateTimeNow.ToString("dd/MM/yyyy");
            var time = dateTimeNow.ToString("HH:mm:ss");

            DateTimeFormat = string.Format($"{time} {day} {date}");
        }

        /// <summary>
        /// Loads the image suggestion.
        /// </summary>
        /// <param name="nameImage">The name image.</param>
        /// <returns></returns>
        protected BitmapImage LoadImageSuggestion(string nameImage)
        {
            try
            {
                var uriImage = new Uri(Path.Combine(EnvironmentPath.PathImageStore, nameImage ?? _imageDefault));
                return new BitmapImage(uriImage);
            }
            catch (FileNotFoundException)
            {
                var uriImage = new Uri(Path.Combine(EnvironmentPath.PathImageStore, _imageDefault));
                return new BitmapImage(uriImage);
            }
        }

        /// <summary>
        /// Calls the oxforddictionaries API.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="option">The option.</param>
        /// <returns></returns>
        protected async Task<JObject> CallOxforddictionariesApi(string word)
        {
            JObject respone = null;
            const string lang_code = "en-us";
            const string fields = "pronunciations";
            const string strictMatch = "false";
            string WEBSERVICE_URL = "https://od-api.oxforddictionaries.com:443/api/v2/entries/" + lang_code + '/' + word + "?fields=" + fields + "&strictMatch=" + strictMatch;

            var webRequest = System.Net.WebRequest.Create(WEBSERVICE_URL);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("app_id", "07fff40b");
                webRequest.Headers.Add("app_key", "ca2a65a610aa164c4d0eda334bef2de7");

                using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                    {
                        var data = await sr.ReadToEndAsync();
                        respone = JObject.Parse(data);
                    }
                }
            }

            return respone;
        }
        #endregion Methods
    }
}