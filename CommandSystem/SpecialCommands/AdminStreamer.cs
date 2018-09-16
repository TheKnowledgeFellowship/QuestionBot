using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class AdminStreamer : ISpecialCommand
    {
        public string Name => "admin-streamer";
        public string Call => @"^admin\s*streamer\s*";
        public Platform Platform => Platform.Discord;

        public async Task ActionAsync(SpecialCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Admin Streamer command provides sub-commands to manage the streamer, that use and or want to use the bot. Usage: `(prefix) Admin Streamer [ permit / remove ]`";
    }
}
