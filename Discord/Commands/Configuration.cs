using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using QuestionBot.ItemsJson;
using QuestionBot.Models;

namespace QuestionBot.Discord.Commands
{
    public class Configuration
    {
        private ItemsJson<Streamer> _streamer;

        public Configuration(ConfigurationDependencies dep)
        {
            _streamer = dep.Streamer;
        }

        [Command("SetRecognitionMode"), Aliases("srm"), Description("With this command you can set, how QuestionBot recognises questions. You have the following options: byKeywords, byCommand, both. Usage: `!srm [byKeywords / byCommand / both]")]
        public async Task SetRecognitionMode(CommandContext context)
        {
            Logger.Console.LogCommand("SetRecognitionMode", context);

            var id = context.User.Id;
            var streamer = _streamer.Items.SingleOrDefault(s => s.DiscordId == id);

            if (streamer == null)
            {
                await Logger.Console.ResponseLogAsync("Sorry, only enabled streamers can use this command.", context);
                return;
            }

            var arguments = context.RawArgumentString.Split(" ");
            if (arguments.Count() < 2)
            {
                await Logger.Console.ResponseLogAsync("You didn't provide enough arguments. Command usage: `!srm [byKeywords / byCommand / both]`", context);
                return;
            }

            var argument = arguments[1].Trim().ToLower();
            QuestionRecognitionMode recognitionMode;

            switch (argument)
            {
                case "both":
                    recognitionMode = QuestionRecognitionMode.Both;
                    break;
                case "bykeywords":
                    recognitionMode = QuestionRecognitionMode.ByKeywords;
                    break;
                case "bycommand":
                    recognitionMode = QuestionRecognitionMode.ByCommand;
                    break;
                default:
                    await Logger.Console.ResponseLogAsync("Sorry, the argument you provided can't be read. Command usage: `!srm [byKeywords / byCommand / both]`.", context);
                    return;
            }

            streamer.QuestionRecognitionMode = recognitionMode;
            await Logger.Console.ResponseLogAsync($"You successfully set the question recognition mode to {recognitionMode}", context);
        }
    }
}
