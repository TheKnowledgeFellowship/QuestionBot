using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionUnansweredId : IStreamerCommand
    {
        public string Name => "question-unanswered-id";
        public string Call => @"^question\s+unanswered\s+\d+";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Moderator;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Moderator;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^question\s+answered\s+", RegexOptions.IgnoreCase);
            var arguments = commandArguments.CommandText.Remove(match.Index, match.Length).Trim().Split(" ");

            int questionId;
            if (!int.TryParse(arguments[0], out questionId))
            {
                await commandArguments.Client.SendMessageAsync($"The argument you provided could not be recognized.");
                return;
            }

            using (var db = new CuriosityContext())
            {
                var question = db.Questions
                    .Where(q => q.Streamer.Id == commandArguments.Streamer.Id)
                    .SingleOrDefault(q => q.ReadableId == questionId);

                if (question != null)
                {
                    question.Answered = false;
                    await db.SaveChangesAsync();
                    await commandArguments.Client.SendMessageAsync($"Question #{questionId} has been successfully marked as unanswered.");
                }
                else
                    await commandArguments.Client.SendMessageAsync($"Question #{questionId} could not be marked as unanswered. Are you sure, that the question exists?");
            }
        }
    }
}
