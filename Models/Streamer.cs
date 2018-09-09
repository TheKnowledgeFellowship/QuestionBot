using System;
using QuestionBot.ItemsJson;

namespace QuestionBot.Models
{
    public enum QuestionRecognitionMode
    {
        ByCommand,
        ByKeywords,
        Both
    }

    public class Streamer : IIdentifier
    {
        public Streamer(ulong discordId, ulong discordChannel, string twitchChannelName)
        {
            this.DiscordId = discordId;
            this.DiscordChannel = discordChannel;
            this.TwitchChannelName = twitchChannelName;
        }

        public long Id { get; set; }
        public ulong DiscordId { get; set; }
        public ulong DiscordChannel { get; set; }
        public string TwitchChannelName { get; set; }
        public string TwitchClientId { get; set; } = null;
        public QuestionRecognitionMode QuestionRecognitionMode { get; set; } = QuestionRecognitionMode.Both;
        public char TwitchCommandPrefix { get; set; } = '!';
    }
}
