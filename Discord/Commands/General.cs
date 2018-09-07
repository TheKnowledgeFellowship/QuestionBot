using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using QuestionBot.ItemsJson;
using QuestionBot.Models;

namespace QuestionBot.Discord.Commands
{
    public class General
    {
        private ulong _admin;
        private ItemsJson<StreamerId> _permittedStreamerIds;
        private ItemsJson<Streamer> _streamer;

        public General(GeneralDependencies dep)
        {
            var config = Config.Config.Load();
            _admin = config.Discord.Admin;
            this._permittedStreamerIds = dep.PermittedStreamerIds;
            this._streamer = dep.Streamer;
        }

        [Command("Enable"), Aliases("e"), Description("Enable QuestionBot for yourself. Use this command like the following: `!enable [Twitch Channel Name] [Discord Channel Id]")]
        public async Task Enable(CommandContext context)
        {
            if (!_permittedStreamerIds.Items.Exists(psi => psi.DiscordId == context.User.Id))
            {
                await context.RespondAsync("Sorry, you aren't a permitted streamer. Please contact jspp for help.");
                return;
            }
            else
            {
                var arguments = context.RawArgumentString.Split(" ");

                if (arguments.Count() < 3)
                {
                    await context.RespondAsync("Error. You did not provide enough arguments.");
                    return;
                }

                var twitchChannelName = arguments[1];
                ulong discordChannelId;
                if (!ulong.TryParse(arguments[2], out discordChannelId))
                {
                    await context.RespondAsync("Error. The provided ChannelId is not an id.");
                    return;
                }

                // Check if the Discord Id already enabled QuestionBot.
                var potentialStreamer = _streamer.Items.SingleOrDefault(s => s.DiscordId == context.User.Id);
                if (potentialStreamer != null)
                {
                    await context.RespondAsync($"You already enabled QuestionBot with the following values: [ TwitchChannelName:{potentialStreamer.TwitchChannelName} ] [ DiscordChannel:{potentialStreamer.DiscordChannel} ]");
                    return;
                }

                var streamer = new Streamer(context.User.Id, discordChannelId, twitchChannelName);
                await _streamer.AddItemAsync(streamer);
                await context.RespondAsync($"You successfully enabled QuestionBot with the following values: [ TwitchChannelName: {streamer.TwitchChannelName} ] [ DiscordChannel: {streamer.DiscordChannel} ]");
            }
        }

        [Command("AddPermittedStreamerId"), Aliases("apsi"), Description("Add a permitted streamer id.")]
        public async Task AddPermittedStreamerId(CommandContext context)
        {
            if (context.User.Id != _admin)
            {
                await context.RespondAsync("Only the Admin can do this.");
            }
            else
            {
                var arguments = context.RawArgumentString.Split(" ");
                ulong output;

                if (ulong.TryParse(arguments[1], out output))
                {
                    if (_permittedStreamerIds.Items.Exists(psi => psi.DiscordId == output))
                    {
                        await context.RespondAsync($"The id {output} is already in the permitted streamer ids.");
                        return;
                    }

                    await _permittedStreamerIds.AddItemAsync(new StreamerId(output));
                    await context.RespondAsync($"You successfully added {output} to the permitted streamer ids.");
                    return;
                }

                await context.RespondAsync($"Error. The first argument has to be an id.");
            }
        }
    }
}
