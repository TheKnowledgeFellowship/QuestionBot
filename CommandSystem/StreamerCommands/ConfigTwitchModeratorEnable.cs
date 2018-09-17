using System.Linq;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigTwitchModeratorEnable : IStreamerCommand
    {
        public string Name => "config-twitchmoderator-enable";
        public string Call => @"^config\s*twitchmoderator\s*enable\s*";
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
                    await commandArguments.Client.SendMessageAsync("Your Twitch moderators are already enabled to execute all commands, that have the permission level set to moderator for Twitch.");
                else
                {
                    streamer.TwitchModeratorEnabled = true;
                    await db.SaveChangesAsync();
                    await commandArguments.Client.SendMessageAsync("You successfully enabled your Twitch moderators to execute all commands, that have the permission level set to moderator for Twitch.");
                }
            }
        }
    }
}
