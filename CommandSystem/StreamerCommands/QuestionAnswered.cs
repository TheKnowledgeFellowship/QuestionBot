using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionAnswered : IStreamerCommand
    {
        public string Name => "question-answered";
        public string Call => @"^question\s*answered\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Moderator;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Moderator;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Question Answered command enables you to mark questions as answered. Usage: `(prefix)Question Answered [ all / question id ]`.";
    }
}
