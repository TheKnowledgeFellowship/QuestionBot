using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class AdminStreamerPermit : ISpecialCommand
    {
        public string Name => "admin-streamer-permit";
        public string Call => @"^admin\s+streamer\s+permit\s*";
        public Platform Platform => Platform.Discord;

        public async Task ActionAsync(SpecialCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Admin Streamer Permit command enables you to permit a streamer to use QuestionBot. Usage: `(prefix)Admin Streamer Permit [ streamer id ]`.";
    }
}
