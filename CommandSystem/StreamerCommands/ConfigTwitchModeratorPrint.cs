using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigTwitchModeratorPrint : IStreamerCommand
    {
        public string Name => "config-twitchmoderator-print";
        public string Call => @"^config\s+twitchmoderator\s+print\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            using (var db = new CuriosityContext())
            {
                var streamer = db.Streamer
                    .Single(s => s.Id == commandArguments.Streamer.Id);

                if (streamer.TwitchModeratorEnabled)
                    await commandArguments.Client.SendMessageAsync("Your Twitch moderators are enabled to execute all commands, that have the permission level set to moderator for Twitch.");
                else
                    await commandArguments.Client.SendMessageAsync("Your Twitch moderators are disabled from executing all commands, that have the permission level set to moderator for Twitch.");
            }
        }
    }
}
