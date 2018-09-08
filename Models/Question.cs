using System;
using QuestionBot.ItemsJson;

namespace QuestionBot.Models
{
    public class Question : IIdentifier
    {
        public Question(ulong streamerId, string content, string author, DateTime time)
        {
            this.StreamerId = streamerId;
            this.Content = content;
            this.Author = author;
            this.Time = time;
        }

        public long Id { get; set; } = 0;
        public ulong StreamerId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }
        public bool Answered { get; set; } = false;
        public bool WhileLive { get; set; } = false;

        public override string ToString() => $"[{Time.ToString("s")}] {Author} asked \"{Content}\" #{Id}";
    }
}
