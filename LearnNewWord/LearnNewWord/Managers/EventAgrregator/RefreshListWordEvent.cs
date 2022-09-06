using LearnNewWord.Managers.EventAgrregator.Payload;
using Prism.Events;

namespace LearnNewWord.Managers.EventAgrregator
{
    public class RefreshListWordEvent : PubSubEvent<SettingPayload>
    {
    }
}