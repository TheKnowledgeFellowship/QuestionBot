using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionRemoveUnanswered : IStreamerCommand
    {
        public string Name => "question-remove-unanswered";
        public string Call => @"^question\s*remove\s*unanswered\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions
                    .Where(q => q.Streamer.Id == commandArguments.Streamer.Id)
                    .Where(q => !q.Answered)
                    .ToList();

                db.Questions.RemoveRange(questions);
                await db.SaveChangesAsync();
            }
            await commandArguments.Client.SendMessageAsync("All unanswered questions got removed successfully.");
        }
    }
}
