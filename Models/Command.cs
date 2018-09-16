using QuestionBot.CommandSystem;

namespace QuestionBot.Models
{
    public class Command
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public PermissionLevel? DiscordPermissionLevel { get; set; } = null;
        public PermissionLevel? TwitchPermissionLevel { get; set; } = null;

        public int StreamerId { get; set; }
        public Streamer Streamer { get; set; }
    }
}
