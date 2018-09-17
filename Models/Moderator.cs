namespace QuestionBot.Models
{
    public class Moderator
    {
        public Moderator(ulong discordId, int streamerId)
        {
            this.DiscordId = discordId;
            this.StreamerId = streamerId;
        }

        public int Id { get; set; }
        public ulong DiscordId { get; set; }

        public int StreamerId { get; set; }
        public Streamer Streamer { get; set; }
    }
}
