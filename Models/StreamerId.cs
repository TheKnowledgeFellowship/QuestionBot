using QuestionBot.ItemsJson;

namespace QuestionBot.Models
{
    public class StreamerId : IIdentifier
    {
        public StreamerId(ulong discordId)
        {
            this.DiscordId = discordId;
        }

        public long Id { get; set; }
        public ulong DiscordId { get; set; }
    }
}
