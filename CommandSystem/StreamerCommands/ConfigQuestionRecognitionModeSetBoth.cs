using System.Linq;
using System.Threading.Tasks;
using QuestionBot.Models;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigQuestionRecognitionModeSetBoth : IStreamerCommand
    {
        public string Name => "config-questionrecognitionmode-set-both";
        public string Call => @"^config\s+questionrecognitionmode\s+set\s+both\s*";
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
                    streamer.QuestionRecognitionMode = QuestionRecognitionMode.Both;
                    await db.SaveChangesAsync();
                    await commandArguments.Client.SendMessageAsync("The QuestionRecognitionMode has been successfully changed to `both`.");
                }
            }
        }
    }
}
