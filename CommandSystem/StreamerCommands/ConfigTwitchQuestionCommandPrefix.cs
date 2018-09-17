using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigTwitchQuestionCommandPrefix : IStreamerCommand
    {
        public string Name => "config-twitchquestioncommandprefix";
        public string Call => @"^config\s+twitchquestioncommandprefix\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config TwitchQuestionCommandPrefix command provides sub-commands to set the prefix character of the `q` and `question` command for the Twitch chat. You can also print out the current prefix. Usage: `(prefix)Config TwitchQuestionCommandPrefix [ set [ character / default ] / print ]`.";
    }
}
