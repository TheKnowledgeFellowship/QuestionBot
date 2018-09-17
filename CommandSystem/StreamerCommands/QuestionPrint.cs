using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionPrint : IStreamerCommand
    {
        public string Name => "question-print";
        public string Call => @"^question\s+print\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Moderator;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Moderator;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Question Print command enables you to print out questions. Usage: `(prefix)Question Print [ all / question id / answered / unanswered ]`.";
    }
}
