using System;

namespace Common.Dto
{
    public class WordDto
    {
        public int Id { get; set; }

        public string English { get; set; }

        public string IpaText { get; set; }

        public string Vietnamese { get; set; }

        public string Example { get; set; }

        public string Image { get; set; }

        public int Noun { get; set; }

        public int Ajective { get; set; }

        public int Verb { get; set; }

        public int Adverb { get; set; }


        public int Known { get; set; }

        public int CategoryId { get; set; }

        public int Total { get; set; }
    }
}