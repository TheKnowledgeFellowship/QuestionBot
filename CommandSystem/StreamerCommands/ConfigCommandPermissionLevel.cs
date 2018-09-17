using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigCommandPermissionLevel : IStreamerCommand
    {
        public string Name => "config-commandpermissionlevel";
        public string Call => @"^config\s*commandpermissionlevel\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config CommandPermissionLevel command enables you to set the permission level for certain commands to certain permission levels. It can also print out all commands, that support a specified permission level. Usage: `(prefix)Config CommandPermissionLevel [ set [ command + platform + permission level ] / print [ streamer / moderator / everyone ] ]`";
    }
}
