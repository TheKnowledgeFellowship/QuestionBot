using QuestionBot.Models;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class QuestionBotEnabledArgs
    {
        public Streamer Streamer { get; set; }

        public QuestionBotEnabledArgs(Streamer streamer) => this.Streamer = streamer;
    }
}
