using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigQuestionRecognitionModePrint : IStreamerCommand
    {
        public string Name => "config-questionrecognitionmode-print";
        public string Call => @"^config\s+questionrecognitionmode\s+print\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            using (var db = new CuriosityContext())
            {
                var streamer = db.Streamer
                    .SingleOrDefault(s => s.Id == commandArguments.Streamer.Id);

                if (streamer == null)
                    await commandArguments.Client.SendMessageAsync("An error occured.");
                else
                {
                    await commandArguments.Client.SendMessageAsync($"The currently active QuestionRecognitionMode is `{streamer.QuestionRecognitionMode}`.");
                }
            }
        }
    }
}
