using Prism.Mvvm;
using System;

namespace LearnNewWord.Model
{
    public class WordModel : BindableBase, ICloneable
    {
        #region Fields

        private int _id;
        private string _english;
        private string _ipaText;
        private string _vietnamese;
        private string _example;
        private string _nameImage;
        private bool _isNoun;
        private bool _isAjective;
        private bool _isVerb;
        private bool _isAdverb;
        private bool _isOther;
        private bool _isKnown;

        #endregion Fields

        #region Properties

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string English
        {
            get => _english;
            set => SetProperty(ref _english, value);
        }

        public string IpaText
        {
            get => _ipaText;
            set => SetProperty(ref _ipaText, value);
        }

        public string Vietnamese
        {
            get => _vietnamese;
            set => SetProperty(ref _vietnamese, value);
        }

        public string Example
        {
            get => _example;
            set => SetProperty(ref _example, value);
        }

        public string Image
        {
            get => _nameImage;
            set => SetProperty(ref _nameImage, value);
        }

        public bool IsNoun
        {
            get => _isNoun;
            set => SetProperty(ref _isNoun, value);
        }

        public bool IsAjective
        {
            get => _isAjective;
            set => SetProperty(ref _isAjective, value);
        }

        public bool IsVerb
        {
            get => _isVerb;
            set => SetProperty(ref _isVerb, value);
        }

        public bool IsAdverb
        {
            get => _isAdverb;
            set => SetProperty(ref _isAdverb, value);
        }

        public bool IsOther
        {
            get => _isOther;
            set => SetProperty(ref _isOther, value);
        }

        public bool IsKnown
        {
            get => _isKnown;
            set => SetProperty(ref _isKnown, value);
        }

        #endregion Properties

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}