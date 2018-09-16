using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class Question : IStreamerCommand
    {
        public string Name => "question";
        public string Call => @"^question\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.everyone;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.everyone;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Question command provides sub-commands, that can do all kind of question related stuff. Usage: `(prefix)Question [ Print / Answered / Unanswered ]`.";
    }
}
