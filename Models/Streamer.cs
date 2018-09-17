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
        public Streamer(ulong discordId, ulong discordChannel, ulong discordGuild, string twitchChannelName)
        {
            this.DiscordId = discordId;
            this.DiscordChannel = discordChannel;
            this.DiscordGuild = discordGuild;
            this.TwitchChannelName = twitchChannelName;
        }

        public int Id { get; set; }
        // Discord data.
        public ulong DiscordId { get; set; }
        public ulong DiscordChannel { get; set; }
        public ulong DiscordGuild { get; set; }
        // Twitch data.
        public string TwitchChannelName { get; set; }
        public string TwitchClientId { get; set; }
        // Config.
        public QuestionRecognitionMode QuestionRecognitionMode { get; set; } = QuestionRecognitionMode.Both;
        public char TwitchCommandPrefix { get; set; } = '!';
        public bool TwitchModeratorEnabled { get; set; } = false;

        public List<Question> Questions { get; set; }
    }
}
