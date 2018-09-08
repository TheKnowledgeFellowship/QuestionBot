using System;
using QuestionBot.Models;

namespace QuestionBot.Discord
{
    public class QuestionBotEnabledArgs
    {
        public Streamer Streamer { get; set; }

        public QuestionBotEnabledArgs(Streamer streamer) => this.Streamer = streamer;
    }

    public class CommandsEvents
    {
        public event EventHandler<QuestionBotEnabledArgs> QuestionBotEnabled;

        public virtual void OnQuestionBotEnabled(Streamer streamer) => QuestionBotEnabled?.Invoke(this, new QuestionBotEnabledArgs(streamer));
    }
}
