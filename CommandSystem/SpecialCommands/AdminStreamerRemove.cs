using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class AdminStreamerRemove : ISpecialCommand
    {
        public string Name => "admin-streamer-remove";
        public string Call => @"^admin\s*streamer\s*remove\s*";
        public Platform Platform => Platform.Discord;

        public async Task ActionAsync(SpecialCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Admin Streamer Remove command enables you to remove a streamer from the permitted streamers, while deleting all information connected to the streamer. Usage: `(prefix)Admin Streamer Remove [ streamer id ]`.";
    }
}
