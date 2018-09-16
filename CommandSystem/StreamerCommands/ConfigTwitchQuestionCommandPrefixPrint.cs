using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigTwitchQuestionCommandPrefixPrint : IStreamerCommand
    {
        public string Name => "config-twitchquestioncommandprefix-print";
        public string Call => @"^config\s*twitchquestioncommandprefix\s*print\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            using (var db = new CuriosityContext())
            {
                var streamer = db.Streamer
                    .SingleOrDefault(s => s.Id == commandArguments.Streamer.Id);

                if (streamer == null)
                    await commandArguments.Client.SendMessageAsync("An error occured.");
                else
                    await commandArguments.Client.SendMessageAsync($"The current TwitchQuestionCommandPrefix is `{streamer.TwitchCommandPrefix}`");
            }
        }
    }
}
