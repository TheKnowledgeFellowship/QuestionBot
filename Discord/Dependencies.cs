using System.Collections.Generic;
using QuestionBot.ItemsJson;
using QuestionBot.Models;

namespace QuestionBot.Discord
{
    public class GeneralDependencies
    {
        internal ItemsJson<StreamerId> PermittedStreamerIds { get; set; }
        internal ItemsJson<Streamer> Streamer { get; set; }
        internal CommandsEvents CommandsEvents { get; set; }
    }

    public class QuestionDependencies
    {
        internal Dictionary<ulong, ItemsJson<Question>> Questions { get; set; }
    }
}
