using System;
using QuestionBot.ItemsJson;

namespace QuestionBot.Models
{
    public class Question : IIdentifier
    {
        public long Id { get; set; }
        public ulong StreamerId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }
    }
}
