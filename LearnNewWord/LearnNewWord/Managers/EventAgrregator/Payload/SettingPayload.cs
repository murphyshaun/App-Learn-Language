using LearnNewWord.Model;
using System.Collections.Generic;

namespace LearnNewWord.Managers.EventAgrregator.Payload
{
    public class SettingPayload
    {
        public int FromIndex { get; set; }

        public int ToIndex { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<WordModel> ListWordPractice { get; set; }

        public IEnumerable<WordModel> AllWord { get; set; }

        public bool IsRandom { get; set; }
    }
}