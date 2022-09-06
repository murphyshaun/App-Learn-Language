using Prism.Mvvm;

namespace LearnNewWord.Managers.Resouce.Model
{
    public class KeyValueGeneric<TKey, TValue> : BindableBase
    {
        private TKey _key;
        private TValue _value;

        public TKey Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public TValue Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}