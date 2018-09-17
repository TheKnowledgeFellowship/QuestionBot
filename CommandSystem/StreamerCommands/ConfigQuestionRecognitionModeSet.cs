using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigQuestionRecognitionModeSet : IStreamerCommand
    {
        public string Name => "config-questionrecognitionmode-set";
        public string Call => @"^config\s+questionrecognitionmode\s+set\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config QuestionRecognitionMode command enables you to set the mode, the bot uses to recognise questions in the Twitch chat. Usage: `(prefix)Config QuestionRecognitionMode set [ byCommand / byKeywords / both / default ]`.";
    }
}
