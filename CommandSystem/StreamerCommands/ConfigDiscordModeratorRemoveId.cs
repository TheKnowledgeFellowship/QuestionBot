using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigDiscordModeratorRemoveId : IStreamerCommand
    {
        public string Name => "config-discordmoderator-remove-id";
        public string Call => @"^config\s+discordmoderator\s+remove\s+\d+";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^config\s+discordmoderator\s+remove\s+", RegexOptions.IgnoreCase);
            var arguments = commandArguments.CommandText.Remove(match.Index, match.Length).Trim().Split(" ");

            ulong discordId;
            if (!ulong.TryParse(arguments[0], out discordId))
            {
                await commandArguments.Client.SendMessageAsync($"The argument you provided could not be recognized.");
                return;
            }

            using (var db = new CuriosityContext())
            {
                var moderator = db.Moderators
                    .Where(m => m.Streamer.Id == commandArguments.Streamer.Id)
                    .SingleOrDefault(m => m.DiscordId == discordId);

                if (moderator == null)
                    await commandArguments.Client.SendMessageAsync($"The Discord user with the id {discordId} is not a moderator.");
                else
                {
                    db.Moderators.Remove(moderator);
                    await db.SaveChangesAsync();
                    await commandArguments.Client.SendMessageAsync($"The Discord user with the id {discordId} has been successfully removed from the moderator.");
                }
            }
        }
    }
}
