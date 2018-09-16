using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigQuestionRecognitionMode : IStreamerCommand
    {
        public string Name => "config-questionrecognitionmode";
        public string Call => @"^config\s*questionrecognitionmode\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config QuestionRecognitionMode command provides sub-commands to set the mode, the bot uses to recognise questions in the Twitch chat. You can also print out the current mode. Usage: `(prefix)Config QuestionRecognitionMode [ set [ byCommand / byKeywords / both / default ] / print ]`.";
    }
}
