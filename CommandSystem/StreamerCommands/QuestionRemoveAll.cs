using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionRemoveAll : IStreamerCommand
    {
        public string Name => "question-remove-all";
        public string Call => @"^question\s*remove\s*all\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions
                    .Where(q => q.Streamer.Id == commandArguments.Streamer.Id)
                    .ToList();

                db.Questions.RemoveRange(questions);
                await db.SaveChangesAsync();
            }
            await commandArguments.Client.SendMessageAsync("All questions got removed successfully.");
        }
    }
}
