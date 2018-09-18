using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigCommandPermissionLevelPrint : IStreamerCommand
    {
        public string Name => "config-commandpermissionlevel-print";
        public string Call => @"^config\s+commandpermissionlevel\s+print\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.Discord;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config CommandPermissionLevel Print command prints all commands, that support the specified permission level. It also shows the current permission level for each command. Usage: `Config CommandPermissionLevel Print [ streamer / moderator / everyone ]`";
    }
}
