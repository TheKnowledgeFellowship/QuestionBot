using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using QuestionBot.ItemsJson;

namespace QuestionBot.Discord.Commands
{
    public class Question
    {
        private Dictionary<ulong, ItemsJson<Models.Question>> _questions;

        public Question(QuestionDependencies dep)
        {
            _questions = dep.Questions;
        }

        [Command("PrintQuestions"), Aliases("pq"), Description("Prints out all unanswered questions.")]
        public async Task PrintQuestions(CommandContext context)
        {
            Logger.Console.LogCommand("PrintQuestions", context);

            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            var response = "All unanswered questions:\n";
            foreach (var question in _questions[id].Items)
            {
                if (!question.Answered)
                    response += $"{question.ToString()}\n";
            }

            await Logger.Console.ResponseLogAsync(response, context);
        }

        [Command("PrintAllQuestions"), Aliases("paq"), Description("Prints out all questions.")]
        public async Task PrintAllQuestions(CommandContext context)
        {
            Logger.Console.LogCommand("PrintAllQuestions", context);

            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            var response = "All questions:\n";
            foreach (var question in _questions[id].Items)
            {
                if (!question.Answered)
                    response += $"{question.ToString()}\n";
                else
                    response += $"{question.ToString()} [answered]\n";
            }

            await Logger.Console.ResponseLogAsync(response, context);
        }

        [Command("Answered"), Aliases("a"), Description("Marks a question as answered.")]
        public async Task Answered(CommandContext context)
        {
            Logger.Console.LogCommand("Answered", context);

            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to mark as answered.", context);
                return;
            }

            long questionId;
            if (!long.TryParse(arguments[1], out questionId))
            {
                await Logger.Console.ResponseLogAsync($"The Id you provided can't be read.", context);
                return;
            }

            var question = _questions[id].Items.SingleOrDefault(q => q.Id == questionId);

            if (question == null)
            {
                await Logger.Console.ResponseLogAsync($"Sorry, no question with the specified Id could be found.", context);
                return;
            }

            question.Answered = true;
            await _questions[id].UpdateItemAsync(question.Id, question);
            await Logger.Console.ResponseLogAsync($"Question #{question.Id} has been marked as answered.", context);
        }

        [Command("Unanswered"), Aliases("u"), Description("Marks a question as unanswered.")]
        public async Task Unanswered(CommandContext context)
        {
            Logger.Console.LogCommand("Unanswered", context);

            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to mark as unanswered.", context);
                return;
            }

            long questionId;
            if (!long.TryParse(arguments[1], out questionId))
            {
                await Logger.Console.ResponseLogAsync($"The Id you provided can't be read.", context);
                return;
            }

            var question = _questions[id].Items.SingleOrDefault(q => q.Id == questionId);

            if (question == null)
            {
                await Logger.Console.ResponseLogAsync($"Sorry, no question with the specified Id could be found.", context);
                return;
            }

            question.Answered = false;
            await _questions[id].UpdateItemAsync(question.Id, question);
            await Logger.Console.ResponseLogAsync($"Question #{question.Id} has been marked as unanswered.", context);
        }

        [Command("Print"), Aliases("p"), Description("Prints a question.")]
        public async Task Print(CommandContext context)
        {
            Logger.Console.LogCommand("Print", context);

            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await Logger.Console.ResponseLogAsync($"Please provide the id of the question you want to print out.", context);
                return;
            }

            long questionId;
            if (!long.TryParse(arguments[1], out questionId))
            {
                await Logger.Console.ResponseLogAsync($"The Id you provided can't be read.", context);
                return;
            }

            var question = _questions[id].Items.SingleOrDefault(q => q.Id == questionId);

            if (question == null)
            {
                await Logger.Console.ResponseLogAsync($"Sorry, no question with the specified Id could be found.", context);
                return;
            }

            await Logger.Console.ResponseLogAsync(question.ToString(), context);
        }

        [Command("LastHours"), Aliases("lh"), Description("Prints all questions, that got asked in the last X hours. X gets specified by you.")]
        public async Task LastHours(CommandContext context)
        {
            Logger.Console.LogCommand("LastHours", context);

            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
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

            var questions = _questions[id].Items.Where(h => h.Time > limit);

            if (questions.Count() == 0)
            {
                await Logger.Console.ResponseLogAsync($"There are no questions, that got asked in the last {hours} hours.", context);
                return;
            }

            var response = $"All questions of the last {hours} hours:\n";
            foreach (var question in questions)
            {
                if (!question.Answered)
                    response += $"{question.ToString()}\n";
                else
                    response += $"{question.ToString()} [answered]\n";
            }

            await Logger.Console.ResponseLogAsync(response, context);
        }

        [Command("RemoveAll"), Aliases("ra"), Description("Removes all questions.")]
        public async Task RemoveAll(CommandContext context)
        {
            Logger.Console.LogCommand("RemoveAll", context);

            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await Logger.Console.ResponseLogAsync($"Sorry, you aren't a registered streamer that got any questions.", context);
                return;
            }

            _questions[id].RemoveAll();
            await Logger.Console.ResponseLogAsync("All questions got removed.", context);
        }

        private bool IsStreamerWithQuestions(ulong id) => _questions.Keys.Any(k => k == id);
    }
}
