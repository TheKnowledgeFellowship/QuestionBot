using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionRemoveId : IStreamerCommand
    {
        public string Name => "question-remove-id";
        public string Call => @"^question\s*remove\s*\d+";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^question\s*remove\s*", RegexOptions.IgnoreCase);
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
                    db.Questions.Remove(question);
                    await db.SaveChangesAsync();
                    await commandArguments.Client.SendMessageAsync($"Question #{questionId} got removed successfully.");
                }
                else
                    await commandArguments.Client.SendMessageAsync($"Question #{questionId} could not be found. Are you sure, that the question exists?");
            }
        }
    }
}
