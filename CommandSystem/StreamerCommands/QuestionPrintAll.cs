using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class QuestionPrintAll : IStreamerCommand
    {
        public string Name => "question-print-all";
        public string Call => @"^question\s+print\s+all\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Moderator;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Moderator;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var response = "All questions:\n";
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions
                    .Where(q => q.Streamer.Id == commandArguments.Streamer.Id)
                    .ToList();

                if (commandArguments.Platform == Platform.Discord)
                {
                    foreach (var question in questions)
                    {
                        if (question.Answered)
                            response += $"{question.ToMarkdownString()} *answered*\n";
                        else
                            response += $"{question.ToMarkdownString()}\n";
                    }
                }
                else
                {
                    foreach (var question in questions)
                    {
                        if (question.Answered)
                            response += $"{question.ToString()} is answered\n";
                        else
                            response += $"{question.ToString()}\n";
                    }
                }
            }
            await commandArguments.Client.SendMessageAsync(response);
        }
    }
}
