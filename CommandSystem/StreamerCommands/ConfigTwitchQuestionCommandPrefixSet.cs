using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigTwitchQuestionCommandPrefixSet : IStreamerCommand
    {
        public string Name => "config-twitchquestioncommandprefix-set";
        public string Call => @"^config\s*twitchquestioncommandprefix\s*set\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config TwitchQuestionCommandPrefix set command enables you to set the prefix character of the `q` and `question` command for the Twitch chat. Usage: `(prefix)Config TwitchQuestionCommandPrefix set [ character / default ]`.";
    }
}
