using System;
using System.Collections.Generic;

namespace QuestionBot.Models
{
    public enum QuestionRecognitionMode
    {
        ByCommand,
        ByKeywords,
        Both
    }

    public class Streamer
    {
        public Streamer(ulong discordId, ulong discordChannel, string twitchChannelName)
        {
            this.DiscordId = discordId;
            this.DiscordChannel = discordChannel;
            this.TwitchChannelName = twitchChannelName;
        }

        public int Id { get; set; }
        public ulong DiscordId { get; set; }
        public ulong DiscordChannel { get; set; }
        public string TwitchChannelName { get; set; }
        public string TwitchClientId { get; set; }
        public QuestionRecognitionMode QuestionRecognitionMode { get; set; } = QuestionRecognitionMode.Both;
        public char TwitchCommandPrefix { get; set; } = '!';
        public bool TwitchModeratorEnabled { get; set; } = false;

        public List<Question> Questions { get; set; }
    }
}
