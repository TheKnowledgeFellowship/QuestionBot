using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigTwitchQuestionCommandPrefixSetChar : IStreamerCommand
    {
        public string Name => "config-twitchquestioncommandprefix-set-char";
        public string Call => @"^config\s+twitchquestioncommandprefix\s+set\s+.\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^config\s+twitchquestioncommandprefix\s+set\s+", RegexOptions.IgnoreCase);
            var arguments = commandArguments.CommandText.Remove(match.Index, match.Length).Trim().Split(" ");

            char commandPrefix;
            if (!char.TryParse(arguments[0], out commandPrefix))
            {
                await commandArguments.Client.SendMessageAsync($"The argument you provided could not be recognized.");
            }
            else
            {
                using (var db = new CuriosityContext())
                {
                    var streamer = db.Streamer
                        .SingleOrDefault(s => s.Id == commandArguments.Streamer.Id);

                    if (streamer == null)
                        await commandArguments.Client.SendMessageAsync("An error occured.");
                    else
                    {
                        streamer.TwitchCommandPrefix = commandPrefix;
                        await db.SaveChangesAsync();
                        await commandArguments.Client.SendMessageAsync($"You successfully set the TwitchQuestionCommandPrefix to `{commandPrefix}`");
                    }
                }
            }
        }
    }
}
