using System;
using QuestionBot.ItemsJson;

namespace QuestionBot.Models
{
    public class Streamer : IIdentifier
    {
        public long Id { get; set; }
        public ulong DiscordId { get; set; }
        public ulong DiscordChannel { get; set; }
        public string TwitchChannelName { get; set; }
    }
}
