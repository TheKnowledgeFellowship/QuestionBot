using System;
using QuestionBot.Models;

namespace QuestionBot.Discord
{
    public class QuestionBotEnabledArgs
    {
        public Streamer Streamer { get; set; }

        public QuestionBotEnabledArgs(Streamer streamer) => this.Streamer = streamer;
    }

    public class RemoveStreamerArgs
    {
        public ulong StreamerId { get; set; }

        public RemoveStreamerArgs(ulong streamerId) => this.StreamerId = streamerId;
    }

    public class CommandsEvents
    {
        public event EventHandler<QuestionBotEnabledArgs> QuestionBotEnabled;
        public event EventHandler<RemoveStreamerArgs> RemoveStreamer;

        public virtual void OnQuestionBotEnabled(Streamer streamer) => QuestionBotEnabled?.Invoke(this, new QuestionBotEnabledArgs(streamer));
        public virtual void OnRemoveStreamer(ulong streamerId) => RemoveStreamer?.Invoke(this, new RemoveStreamerArgs(streamerId));
    }
}
