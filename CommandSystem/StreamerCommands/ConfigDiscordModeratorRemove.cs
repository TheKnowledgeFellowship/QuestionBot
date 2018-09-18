using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigDiscordModeratorRemove : IStreamerCommand
    {
        public string Name => "config-discordmoderator-remove";
        public string Call => @"^config\s+discordmoderator\s+remove\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config DiscordModerator Remove command enables you to remove Discord moderators. Usage: `(prefix)Config DiscordModerator Remove [ id ]`.";
    }
}
