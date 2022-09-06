using LearnNewWord.Model;
using Prism.Events;

namespace LearnNewWord.Managers.EventAgrregator
{
    public class UpdateWordEvent : PubSubEvent<WordModel>
    {
    }
}