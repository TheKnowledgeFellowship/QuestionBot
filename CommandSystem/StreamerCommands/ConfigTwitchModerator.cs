using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigTwitchModerator : IStreamerCommand
    {
        public string Name => "config-twitchmoderator";
        public string Call => @"^config\s*twitchmoderator\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config TwitchModerator command allows you to enable/disable your default Twitch moderators to/from execute/executing all QuestionBot commands, that have the permission level set to Moderator for Twitch. You can also print out the current status. Usage: `(prefix)Config TwitchModerator [ enable / disable / print ]`.";
    }
}
