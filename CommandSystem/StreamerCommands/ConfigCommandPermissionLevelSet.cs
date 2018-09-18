using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigCommandPermissionLevelSet : IStreamerCommand
    {
        public string Name => "config-commandpermissionlevel-set";
        public string Call => @"^config\s+commandpermissionlevel\s+set\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config CommandPermissionLevel Set command enables you to set the permission level for certain commands to certain permission levels. Usage: `(prefix)Config CommandPermissionLevel Set [ command + [ discord / twitch ] + [ streamer / moderator / everyone ] ]";
    }
}
