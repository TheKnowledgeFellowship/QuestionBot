using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionRemoveAnswered : IStreamerCommand
    {
        public string Name => "question-remove-answered";
        public string Call => @"^question\s*remove\s*answered\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions
                    .Where(q => q.Streamer.Id == commandArguments.Streamer.Id)
                    .Where(q => q.Answered)
                    .ToList();

                db.Questions.RemoveRange(questions);
                await db.SaveChangesAsync();
            }
            await commandArguments.Client.SendMessageAsync("All answered questions got removed successfully.");
        }
    }
}
