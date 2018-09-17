using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigTwitchQuestionCommandPrefixSetDefault : IStreamerCommand
    {
        public string Name => "config-twitchquestioncommandprefix-set-default";
        public string Call => @"^config\s+twitchquestioncommandprefix\s+set\s+default\s*";
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
                {
                    streamer.TwitchCommandPrefix = '!';
                    await db.SaveChangesAsync();
                    await commandArguments.Client.SendMessageAsync("You successfully set the TwitchQuestionCommandPrefix to the default value: `!`");
                }
            }
        }
    }
}
