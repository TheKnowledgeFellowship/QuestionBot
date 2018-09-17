using QuestionBot.CommandSystem;

namespace QuestionBot.Models
{
    public class Command
    {
        public Command(string name, int streamerId)
        {
            this.Name = name;
            this.StreamerId = streamerId;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public PermissionLevel? DiscordPermissionLevel { get; set; } = null;
        public PermissionLevel? TwitchPermissionLevel { get; set; } = null;

        public int StreamerId { get; set; }
        public Streamer Streamer { get; set; }
    }
}
