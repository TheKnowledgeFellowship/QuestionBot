using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class Enable : ISpecialCommand
    {
        public string Name => "enable";
        public string Call => @"^enable\s*";
        public Platform Platform => Platform.Discord;

        public async Task ActionAsync(SpecialCommandArguments commandArguments)
        {
            await commandArguments.Client.SendMessageAsync(Help());
        }

        public string Help() => "The Enable command enables permitted streamers to enable QuestionBot. Usage: `(prefix)Enable [ Twitch channel name + Discord question channel name / Twitch channel name + Discord question channel id ]`.";
    }
}
