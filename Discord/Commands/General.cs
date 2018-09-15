using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using QuestionBot.Models;

namespace QuestionBot.Discord.Commands
{
    public class General
    {
        private ulong _admin;
        private CommandsEvents _commandsEvents;

        public General(GeneralDependencies dep)
        {
            var config = Config.Config.Load();
            _admin = config.Discord.Admin;
            this._commandsEvents = dep.CommandsEvents;
        }

        [Command("Enable"), Aliases("e"), Description("Enable QuestionBot for yourself. Use this command like the following: `!enable [Twitch Channel Name] [Discord Channel Name or Id]")]
        public async Task Enable(CommandContext context)
        {
            Logger.Console.LogCommand("Enable", context);

            if (!IsPermitted(context.User.Id))
            {
                await Logger.Console.ResponseLogAsync("Sorry, you aren't a permitted streamer. Please contact jspp for help.", context);
                return;
            }
            else
            {
                if (context.RawArgumentString == null)
                {
                    await Logger.Console.ResponseLogAsync("Error. You did not provide enough arguments.", context);
                    return;
                }

                var arguments = context.RawArgumentString.Split(" ");

                if (arguments.Count() < 3)
                {
                    await Logger.Console.ResponseLogAsync("Error. You did not provide enough arguments.", context);
                    return;
                }

                var gotId = false;
                var twitchChannelName = arguments[1];
                ulong discordChannelId;
                if (ulong.TryParse(arguments[2], out discordChannelId))
                    gotId = true;

                if (!gotId)
                {
                    var allGuildChannels = context.Guild.Channels;
                    var channel = allGuildChannels.SingleOrDefault(c => c.Name.ToLower().Trim() == arguments[2].ToLower().Trim());
                    if (channel != null)
                    {
                        discordChannelId = channel.Id;
                        gotId = true;
                    }
                }

                if (!gotId)
                {
                    await Logger.Console.ResponseLogAsync("Error. The provided Channel is not a channel or ChannelId.", context);
                    return;
                }

                // Check if the Discord Id already enabled QuestionBot.

                Streamer potentialStreamer;
                using (var db = new CuriosityContext())
                {
                    potentialStreamer = db.Streamer
                        .SingleOrDefault(s => s.DiscordId == context.User.Id);
                }
                if (potentialStreamer != null)
                {
                    await Logger.Console.ResponseLogAsync($"You already enabled QuestionBot with the following values: [ TwitchChannelName: {potentialStreamer.TwitchChannelName} ] [ DiscordChannel: {potentialStreamer.DiscordChannel} ]", context);
                    return;
                }

                var streamer = new Streamer(context.User.Id, discordChannelId, twitchChannelName);
                _commandsEvents.OnQuestionBotEnabled(streamer);
                await Logger.Console.ResponseLogAsync($"You successfully enabled QuestionBot with the following values: [ TwitchChannelName: {streamer.TwitchChannelName} ] [ DiscordChannel: {streamer.DiscordChannel} ]", context);
            }
        }

        [Command("AddPermittedStreamerId"), Aliases("apsi"), Description("Add a permitted streamer id.")]
        public async Task AddPermittedStreamerId(CommandContext context)
        {
            Logger.Console.LogCommand("AddPermittedStreamerId", context);

            if (context.User.Id != _admin)
            {
                await Logger.Console.ResponseLogAsync("Only the Admin can do this.", context);
            }
            else
            {
                if (context.RawArgumentString == null)
                {
                    await Logger.Console.ResponseLogAsync("You did not provide enough arguments.", context);
                    return;
                }

                var arguments = context.RawArgumentString.Split(" ");

                if (arguments.Count() < 2)
                {
                    await Logger.Console.ResponseLogAsync("You did not provide enough arguments.", context);
                    return;
                }

                ulong discordId;

                if (ulong.TryParse(arguments[1], out discordId))
                {
                    if (IsPermitted(context.User.Id))
                    {
                        await Logger.Console.ResponseLogAsync($"The id {discordId} is already in the permitted streamer ids.", context);
                        return;
                    }

                    using (var db = new CuriosityContext())
                    {
                        await db.PermittedStreamers.AddAsync(new PermittedStreamer(discordId));
                        await db.SaveChangesAsync();
                    }
                    await Logger.Console.ResponseLogAsync($"You successfully added {discordId} to the permitted streamer ids.", context);
                    return;
                }

                await Logger.Console.ResponseLogAsync($"Error. The first argument has to be an id.", context);
            }
        }

        [Command("RemoveStreamer"), Aliases("rs"), Description("Remove a streamer id from the permitted streamer ids and remove all content associated with the streamer.")]
        public async Task RemoveermittedStreamerId(CommandContext context)
        {
            Logger.Console.LogCommand("RemovePermittedStreamerId", context);

            if (context.User.Id != _admin)
            {
                await Logger.Console.ResponseLogAsync("Only the Admin can do this.", context);
            }
            else
            {
                if (context.RawArgumentString == null)
                {
                    await Logger.Console.ResponseLogAsync("You did not provide enough arguments.", context);
                    return;
                }

                var arguments = context.RawArgumentString.Split(" ");

                if (arguments.Count() < 2)
                {
                    await Logger.Console.ResponseLogAsync("You did not provide enough arguments.", context);
                    return;
                }

                ulong discordId;

                if (ulong.TryParse(arguments[1], out discordId))
                {
                    _commandsEvents.OnRemoveStreamer(discordId);

                    await Logger.Console.ResponseLogAsync($"The streamer has removed from the permitted streamers, if they were part of them. All connected information got removed as well and QuestionBot no longer works for them.", context);
                }
                else
                    await Logger.Console.ResponseLogAsync($"Error. The first argument has to be an id.", context);
            }
        }

        public bool IsPermitted(ulong discordId)
        {
            var isPermitted = false;
            using (var db = new CuriosityContext())
            {
                isPermitted = db.PermittedStreamers
                    .Any(ps => ps.DiscordId == discordId);
            }
            return isPermitted;
        }
    }
}
