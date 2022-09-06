using LearnNewWord.IViewModel;
using LearnNewWord.Managers.Dialog.View;
using LearnNewWord.Managers.Enum;

namespace LearnNewWord.Managers.Dialog
{
    public static class MessageBoxCustom
    {
        public static DialogResult ShowDialog(string header, object content,
            DialogButtonShow dialogButtonShow, int height, int width)
        {
            var dialog = Manager.Resolve<MessageBoxView>();
            var dataContext = Manager.Resolve<IMessageBoxViewModel>();

            dataContext.Header = header;
            dataContext.Content = content;
            dataContext.ButtonShow(dialogButtonShow);
            dataContext.Height = height;
            dataContext.Width = width;

            dialog.DataContext = dataContext;

            dialog.ShowDialog();

            return dataContext.Result;
        }
    }
}