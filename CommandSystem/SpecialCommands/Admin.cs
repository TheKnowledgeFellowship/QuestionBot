using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class Admin : ISpecialCommand
    {
        public string Name => "admin";
        public string Call => @"^admin\s*";
        public Platform Platform => Platform.Discord;

        public async Task ActionAsync(SpecialCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Admin command provides sub-commands to do admin stuff. Usage: `(prefix) Admin [ Streamer ]`";
    }
}
