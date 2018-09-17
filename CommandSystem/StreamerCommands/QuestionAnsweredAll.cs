using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionAnsweredAll : IStreamerCommand
    {
        public string Name => "question-answered-all";
        public string Call => @"^question\s+answered\s+all\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Moderator;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Moderator;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions
                    .Where(q => q.Streamer.Id == commandArguments.Streamer.Id)
                    .ToList();
                foreach (var question in questions)
                    question.Answered = true;
                await db.SaveChangesAsync();
            }
            await commandArguments.Client.SendMessageAsync("All existing questions have been marked as answered.");
        }
    }
}
