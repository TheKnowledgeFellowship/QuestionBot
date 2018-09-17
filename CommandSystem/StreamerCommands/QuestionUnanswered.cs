using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionUnanswered : IStreamerCommand
    {
        public string Name => "question-unanswered";
        public string Call => @"^question\s+unanswered\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Moderator;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Moderator;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Question Unanswered command enables you to mark questions as unanswered. Usage: `(prefix)Question Unanswered [ all / question id ]`.";
    }
}
