using LearnNewWord.Managers.Enum;

namespace LearnNewWord.IViewModel
{
    public interface IMessageBoxViewModel
    {
        string Header { get; set; }

        object Content { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        DialogResult Result { get; set; }

        void ButtonShow(DialogButtonShow dialogButtonShow);
    }
}