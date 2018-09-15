using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace QuestionBot.Discord.Commands
{
    public class Question
    {
        public Question() { }

        [Command("PrintQuestions"), Aliases("pq"), Description("Prints out all unanswered questions.")]
        public async Task PrintQuestions(CommandContext context)
        {
            Logger.Console.LogCommand("PrintQuestions", context);

            var discordId = context.User.Id;

            if (!IsStreamerWithQuestions(discordId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            var response = "All unanswered questions:\n";
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions.Where(q => q.Streamer.DiscordId == discordId);

                foreach (var question in questions)
                {
                    if (!question.Answered)
                        response += $"{question.ToMarkdownString()}\n";
                }
            }
            await Logger.Console.ResponseLogAsync(response, context);
        }

        [Command("PrintAllQuestions"), Aliases("paq"), Description("Prints out all questions.")]
        public async Task PrintAllQuestions(CommandContext context)
        {
            Logger.Console.LogCommand("PrintAllQuestions", context);

            var discordId = context.User.Id;

            if (!IsStreamerWithQuestions(discordId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            var response = "All questions:\n";
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions.Where(q => q.Streamer.DiscordId == discordId);

                foreach (var question in questions)
                {
                    if (!question.Answered)
                        response += $"{question.ToMarkdownString()}\n";
                    else
                        response += $"{question.ToMarkdownString()} *answered*\n";
                }
            }

            await Logger.Console.ResponseLogAsync(response, context);
        }

        [Command("Answered"), Aliases("a"), Description("Marks a question as answered.")]
        public async Task Answered(CommandContext context)
        {
            Logger.Console.LogCommand("Answered", context);

            var discordId = context.User.Id;

            if (!IsStreamerWithQuestions(discordId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            if (context.RawArgumentString == null)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to mark as answered.", context);
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to mark as answered.", context);
                return;
            }

            int questionId;
            if (!int.TryParse(arguments[1], out questionId))
            {
                await Logger.Console.ResponseLogAsync($"The Id you provided can't be read.", context);
                return;
            }

            if (!QuestionExists(discordId, questionId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, no question with the specified Id could be found.", context);
                return;
            }

            using (var db = new CuriosityContext())
            {
                var question = db.Questions
                    .Where(q => q.Streamer.DiscordId == discordId)
                    .Single(q => q.ReadableId == questionId);
                question.Answered = true;
                await db.SaveChangesAsync();
            }
            await Logger.Console.ResponseLogAsync($"Question #{questionId} has been marked as answered.", context);
        }

        [Command("Unanswered"), Aliases("u"), Description("Marks a question as unanswered.")]
        public async Task Unanswered(CommandContext context)
        {
            Logger.Console.LogCommand("Unanswered", context);

            var discordId = context.User.Id;

            if (!IsStreamerWithQuestions(discordId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            if (context.RawArgumentString == null)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to mark as unanswered.", context);
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to mark as unanswered.", context);
                return;
            }

            int questionId;
            if (!int.TryParse(arguments[1], out questionId))
            {
                await Logger.Console.ResponseLogAsync($"The Id you provided can't be read.", context);
                return;
            }

            if (!QuestionExists(discordId, questionId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, no question with the specified Id could be found.", context);
                return;
            }

            using (var db = new CuriosityContext())
            {
                var question = db.Questions
                    .Where(q => q.Streamer.DiscordId == discordId)
                    .Single(q => q.ReadableId == questionId);
                question.Answered = false;
                await db.SaveChangesAsync();
            }
            await Logger.Console.ResponseLogAsync($"Question #{questionId} has been marked as unanswered.", context);
        }

        [Command("Print"), Aliases("p"), Description("Prints a question.")]
        public async Task Print(CommandContext context)
        {
            Logger.Console.LogCommand("Print", context);

            var discordId = context.User.Id;

            if (!IsStreamerWithQuestions(discordId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            if (context.RawArgumentString == null)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to print out.", context);
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to print out.", context);
                return;
            }

            int questionId;
            if (!int.TryParse(arguments[1], out questionId))
            {
                await Logger.Console.ResponseLogAsync($"The Id you provided can't be read.", context);
                return;
            }

            if (!QuestionExists(discordId, questionId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, no question with the specified Id could be found.", context);
                return;
            }

            var response = "";
            using (var db = new CuriosityContext())
            {
                var question = db.Questions
                    .Where(q => q.Streamer.DiscordId == discordId)
                    .Single(q => q.ReadableId == questionId);
                if (!question.Answered)
                    response = question.ToMarkdownString();
                else
                    response = $"{question.ToMarkdownString()} *answered*";
            }

            await Logger.Console.ResponseLogAsync(response, context);
        }

        [Command("LastHours"), Aliases("lh"), Description("Prints all questions, that got asked in the last X hours. X gets specified by you.")]
        public async Task LastHours(CommandContext context)
        {
            Logger.Console.LogCommand("LastHours", context);

            var discordId = context.User.Id;

            if (!IsStreamerWithQuestions(discordId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            if (context.RawArgumentString == null)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the number of hours you want to target.", context);
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the number of hours you want to target.", context);
                return;
            }

            int hours;
            if (!int.TryParse(arguments[1], out hours))
            {
                await Logger.Console.ResponseLogAsync($"The number of hours you provided can't be read.", context);
                return;
            }

            var limit = DateTime.Now - TimeSpan.FromHours(hours);

            var questionsExist = false;
            using (var db = new CuriosityContext())
            {
                questionsExist = db.Questions
                    .Where(q => q.Streamer.DiscordId == discordId)
                    .Any(q => q.Time > limit);
            }

            if (questionsExist)
            {
                await Logger.Console.ResponseLogAsync($"There are no questions, that got asked in the last {hours} hours.", context);
                return;
            }

            var response = $"All questions of the last {hours} hours:\n";
            using (var db = new CuriosityContext())
            {
                var questions = db.Questions
                    .Where(q => q.Streamer.DiscordId == discordId)
                    .Where(q => q.Time > limit);

                foreach (var question in questions)
                {
                    if (!question.Answered)
                        response += $"{question.ToMarkdownString()}\n";
                    else
                        response += $"{question.ToMarkdownString()} *answered*\n";
                }
            }

            await Logger.Console.ResponseLogAsync(response, context);
        }

        [Command("RemoveAll"), Aliases("ra"), Description("Removes all questions.")]
        public async Task RemoveAll(CommandContext context)
        {
            Logger.Console.LogCommand("RemoveAll", context);

            var discordId = context.User.Id;

            if (!IsStreamerWithQuestions(discordId))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            using (var db = new CuriosityContext())
            {
                var questions = db.Questions
                    .Where(q => q.Streamer.DiscordId == discordId);
                db.RemoveRange(questions);
                await db.SaveChangesAsync();
            }

            await Logger.Console.ResponseLogAsync("All questions got removed.", context);
        }

        private bool IsStreamerWithQuestions(ulong discordId)
        {
            var isStreamerWithQuestions = false;
            using (var db = new CuriosityContext())
            {
                isStreamerWithQuestions = db.Questions
                    .Where(q => q.Streamer.DiscordId == discordId)
                    .Any();
            }

            return isStreamerWithQuestions;
        }

        public bool QuestionExists(ulong discordId, int questionId)
        {
            var questionExists = false;
            using (var db = new CuriosityContext())
            {
                questionExists = db.Questions
                    .Where(q => q.Streamer.DiscordId == discordId)
                    .Any(q => q.ReadableId == questionId);
            }

            return questionExists;
        }
    }
}
