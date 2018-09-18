using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigDiscordModerator : IStreamerCommand
    {
        public string Name => "config-discordmoderator";
        public string Call => @"^config\s+discordmoderator\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config DiscordModerator command enables you to add and remove Discord moderators. You can also print out all Discord moderators. Usage: `(prefix)Config DiscordModerator [ Add [ id ] / Remove [ id ] / Print ]`.";
    }
}
