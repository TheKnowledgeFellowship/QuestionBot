using QuestionBot.ItemsJson;
using QuestionBot.Models;

namespace QuestionBot.Discord
{
    public class GeneralDependencies
    {
        internal ItemsJson<StreamerId> PermittedStreamerIds { get; set; }
        internal ItemsJson<Streamer> Streamer { get; set; }
    }
}
