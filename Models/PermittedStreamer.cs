namespace QuestionBot.Models
{
    public class PermittedStreamer
    {
        public PermittedStreamer(ulong discordId)
        {
            this.DiscordId = discordId;
        }

        public int Id { get; set; }
        public ulong DiscordId { get; set; }
    }
}
