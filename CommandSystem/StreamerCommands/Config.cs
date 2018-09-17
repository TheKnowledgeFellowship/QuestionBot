using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class Config : IStreamerCommand
    {
        public string Name => "config";
        public string Call => @"^config\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Config command provides sub-commands to configure different settings. Usage: `(prefix)Config [ QuestionRecognitionMode / TwitchQuestionCommandPrefix / CommandPermissionLevel / DiscordModerator / TwitchModerator ]`.";
    }
}
