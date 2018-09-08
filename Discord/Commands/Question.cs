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
            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await context.RespondAsync($"Sorry, you aren't a registered streamer that got any questions.");
                return;
            }

            var response = "All unanswered questions:\n";
            foreach (var question in _questions[id].Items)
            {
                if (!question.Answered)
                    response += $"{question.ToString()}\n";
            }

            await context.RespondAsync(response);
        }

        [Command("PrintAllQuestions"), Aliases("paq"), Description("Prints out all questions.")]
        public async Task PrintAllQuestions(CommandContext context)
        {
            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await context.RespondAsync($"Sorry, you aren't a registered streamer that got any questions.");
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

            await context.RespondAsync(response);
        }

        [Command("Answered"), Aliases("a"), Description("Marks a question as answered.")]
        public async Task Answered(CommandContext context)
        {
            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await context.RespondAsync($"Sorry, you aren't a registered streamer that got any questions.");
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await context.RespondAsync($"Please provide the id of the question you want to mark as answered.");
                return;
            }

            long questionId;
            if (!long.TryParse(arguments[1], out questionId))
            {
                await context.RespondAsync($"The Id you provided can't be read.");
                return;
            }

            var question = _questions[id].Items.SingleOrDefault(q => q.Id == questionId);

            if (question == null)
            {
                await context.RespondAsync($"Sorry, no question with the specified Id could be found.");
                return;
            }

            question.Answered = true;
            await _questions[id].UpdateItemAsync(question.Id, question);
            await context.RespondAsync($"Question #{question.Id} has been marked as answered.");
        }

        [Command("Unanswered"), Aliases("u"), Description("Marks a question as unanswered.")]
        public async Task Unanswered(CommandContext context)
        {
            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await context.RespondAsync($"Sorry, you aren't a registered streamer that got any questions.");
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await context.RespondAsync($"Please provide the id of the question you want to mark as unanswered.");
                return;
            }

            long questionId;
            if (!long.TryParse(arguments[1], out questionId))
            {
                await context.RespondAsync($"The Id you provided can't be read.");
                return;
            }

            var question = _questions[id].Items.SingleOrDefault(q => q.Id == questionId);

            if (question == null)
            {
                await context.RespondAsync($"Sorry, no question with the specified Id could be found.");
                return;
            }

            question.Answered = false;
            await _questions[id].UpdateItemAsync(question.Id, question);
            await context.RespondAsync($"Question #{question.Id} has been marked as unanswered.");
        }

        [Command("Print"), Aliases("p"), Description("Prints a question.")]
        public async Task Print(CommandContext context)
        {
            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await context.RespondAsync($"Sorry, you aren't a registered streamer that got any questions.");
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await context.RespondAsync($"Please provide the id of the question you want to print out.");
                return;
            }

            long questionId;
            if (!long.TryParse(arguments[1], out questionId))
            {
                await context.RespondAsync($"The Id you provided can't be read.");
                return;
            }

            var question = _questions[id].Items.SingleOrDefault(q => q.Id == questionId);

            if (question == null)
            {
                await context.RespondAsync($"Sorry, no question with the specified Id could be found.");
                return;
            }

            await context.RespondAsync(question.ToString());
        }

        [Command("LastHours"), Aliases("lh"), Description("Prints all questions, that got asked in the last X hours. X gets specified by you.")]
        public async Task Today(CommandContext context)
        {
            var id = context.User.Id;

            if (!IsStreamerWithQuestions(id))
            {
                await context.RespondAsync($"Sorry, you aren't a registered streamer that got any questions.");
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await context.RespondAsync($"Please provide the number of hours you want to target.");
                return;
            }

            int hours;
            if (!int.TryParse(arguments[1], out hours))
            {
                await context.RespondAsync($"The number of hours you provided can't be read.");
                return;
            }

            var limit = DateTime.Now - TimeSpan.FromHours(hours);

            var questions = _questions[id].Items.Where(h => h.Time > limit);

            if (questions.Count() == 0)
            {
                await context.RespondAsync($"There are no questions, that got asked in the last {hours} hours.");
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

            await context.RespondAsync(response);
        }

        private bool IsStreamerWithQuestions(ulong id) => _questions.Keys.Any(k => k == id);
    }
}
