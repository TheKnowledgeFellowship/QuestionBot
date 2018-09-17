using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionRemove : IStreamerCommand
    {
        public string Name => "question-remove";
        public string Call => @"^question\s+remove\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Question Remove command enables you to remove questions. Usage: `(prefix)Question Remove [ all / question id / answered / unanswered ]`.";
    }
}
