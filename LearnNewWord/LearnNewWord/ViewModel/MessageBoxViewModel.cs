using Common.Extension;
using LearnNewWord.IViewModel;
using LearnNewWord.Managers.Enum;
using Prism.Commands;
using Prism.Mvvm;

namespace LearnNewWord.ViewModel
{
    internal class MessageBoxViewModel : BindableBase, IMessageBoxViewModel
    {
        #region Fields

        private string _contentButton1;
        private string _contentButton2;
        private bool _isShowButton1;
        private bool _isShowButton2;
        private int _width;
        private int _height;
        private object _content;
        private string _header;
        private DialogResult _result;

        #endregion Fields

        #region Properties

        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        public object Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public string ContentButton1
        {
            get => _contentButton1;
            set => SetProperty(ref _contentButton1, value);
        }

        public string ContentButton2
        {
            get => _contentButton2;
            set => SetProperty(ref _contentButton2, value);
        }

        public bool IsShowButton1
        {
            get => _isShowButton1;
            set => SetProperty(ref _isShowButton1, value);
        }

        public bool IsShowButton2
        {
            get => _isShowButton2;
            set => SetProperty(ref _isShowButton2, value);
        }

        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public DialogResult Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        #endregion Properties

        #region Commands

        public DelegateCommand<string> ButtonCommand { get; set; }

        #endregion Commands

        #region Constructor

        public MessageBoxViewModel()
        {
            ButtonCommand = new DelegateCommand<string>(ButtonOnClick);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Buttons the show.
        /// </summary>
        /// <param name="dialogButtonShow">The dialog button show.</param>
        public void ButtonShow(DialogButtonShow dialogButtonShow)
        {
            switch (dialogButtonShow)
            {
                case DialogButtonShow.YesNo:
                    {
                        IsShowButton1 = true;
                        IsShowButton2 = true;
                        ContentButton1 = DialogResult.Yes.ToString();
                        ContentButton2 = DialogResult.No.ToString();
                        break;
                    }

                case DialogButtonShow.DeleteCancel:
                    {
                        IsShowButton1 = true;
                        IsShowButton2 = true;
                        ContentButton1 = DialogResult.Delete.ToString();
                        ContentButton2 = DialogResult.Cancel.ToString();
                        break;
                    }

                case DialogButtonShow.OK:
                    {
                        IsShowButton1 = true;
                        ContentButton1 = DialogResult.Ok.ToString();
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// Buttons the on click.
        /// </summary>
        /// <param name="result">The result.</param>
        private void ButtonOnClick(string result)
        {
            Result = result.ToEnum<DialogResult>();
        }

        #endregion Methods
    }
}