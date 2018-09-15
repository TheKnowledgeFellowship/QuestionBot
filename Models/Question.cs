using System;

namespace QuestionBot.Models
{
    public class Question
    {
        public Question(string content, string author, DateTime time, int streamerId)
        {
            this.Content = content;
            this.Author = author;
            this.Time = time;
            this.StreamerId = streamerId;
        }

        public int Id { get; set; }

        public int ReadableId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Time { get; set; }
        public bool Answered { get; set; } = false;
        public bool WhileLive { get; set; } = false;

        public int StreamerId { get; set; }
        public Streamer Streamer { get; set; }

        public override string ToString() => $"[{Time.ToString("s")}] {Author} asked \"{Content}\" #{ReadableId}";

        public string ToMarkdownString() => $"[{Time.ToString("s")}] **{Author}** asked **\"{Content}\"** #{ReadableId}";

        public string ToLightMarkdownString() => $"**{Author}** asked **\"{Content}\"** #{ReadableId}";
    }
}
