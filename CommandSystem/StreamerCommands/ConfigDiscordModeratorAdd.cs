using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigDiscordModeratorAdd : IStreamerCommand
    {
        public string Name => "config-discordmoderator-add";
        public string Call => @"^config\s+discordmoderator\s+add\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config DiscordModerator Add command enables you to add Discord moderators. Usage: `(prefix)Config DiscordModerator Add [ id ]`.";
    }
}
