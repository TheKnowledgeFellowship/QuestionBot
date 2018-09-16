using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionPrintId : IStreamerCommand
    {
        public string Name => "question-print-id";
        public string Call => @"^question\s*print\s*\d+";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Moderator;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Moderator;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^question\s*print\s*", RegexOptions.IgnoreCase);
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
                    if (commandArguments.Platform == Platform.Discord)
                    {
                        if (question.Answered)
                            await commandArguments.Client.SendMessageAsync($"{question.ToMarkdownString()} *answered*");
                        else
                            await commandArguments.Client.SendMessageAsync(question.ToMarkdownString());
                    }
                    else
                    {
                        if (question.Answered)
                            await commandArguments.Client.SendMessageAsync($"{question.ToString()} *answered*");
                        else
                            await commandArguments.Client.SendMessageAsync(question.ToString());
                    }
                }
                else
                    await commandArguments.Client.SendMessageAsync($"Question #{questionId} could not be found. Are you sure, that the question exists?");
            }
        }
    }
}
