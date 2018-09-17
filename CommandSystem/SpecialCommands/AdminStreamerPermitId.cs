using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuestionBot.Models;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class AdminStreamerPermitId : ISpecialCommand
    {
        public string Name => "admin-streamer-permit-id";
        public string Call => @"^admin\s+streamer\s+permit\s+\d+";
        public Platform Platform => Platform.Discord;

        public async Task ActionAsync(SpecialCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^admin\s+streamer\s+permit\s+", RegexOptions.IgnoreCase);
            var arguments = commandArguments.CommandText.Remove(match.Index, match.Length).Trim().Split(" ");

            ulong discordId;
            if (!ulong.TryParse(arguments[0], out discordId))
            {
                await commandArguments.Client.SendMessageAsync($"The argument you provided could not be recognized.");
                return;
            }

            using (var db = new CuriosityContext())
            {
                var permittedStreamer = db.PermittedStreamers
                    .SingleOrDefault(ps => ps.DiscordId == discordId);

                if (permittedStreamer == null)
                {
                    permittedStreamer = new PermittedStreamer(discordId);
                    await db.PermittedStreamers.AddAsync(permittedStreamer);
                    await db.SaveChangesAsync();
                    await commandArguments.Client.SendMessageAsync($"The Discord user with the id `{discordId}` is now a permitted streamer.");
                }
                else
                    await commandArguments.Client.SendMessageAsync($"The Discord user with the id `{discordId}` is already a permitted streamer.");
            }
        }
    }
}
