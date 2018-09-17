using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigDiscordModeratorPrint : IStreamerCommand
    {
        public string Name => "config-discordmoderator-print";
        public string Call => @"^config\s*discordmoderator\s*print\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var response = "All Discord moderators:\n";
            using (var db = new CuriosityContext())
            {
                var moderators = db.Moderators
                    .Where(m => m.Streamer.Id == commandArguments.Streamer.Id)
                    .ToList();

                foreach (var moderator in moderators)
                    response += $"{moderator.DiscordId}\n";
            }
            await commandArguments.Client.SendMessageAsync(response);
        }
    }
}
