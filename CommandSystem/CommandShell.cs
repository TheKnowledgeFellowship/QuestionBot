using System.Linq;
using System.Threading.Tasks;
using QuestionBot.Models;

namespace QuestionBot.CommandSystem
{
    public static class CommandShell
    {
        public static async Task ExecuteStreamerCommandAsync(StreamerCommandArguments commandArguments, IStreamerCommand command)
        {
            var streamerSetPermissionLevel = GetStreamerSetPermissionLevel(command.Name, commandArguments.Streamer, commandArguments.Platform);
            if (commandArguments.Platform == Platform.Twitch)
            {
                if (!(command.Platform == Platform.Twitch || command.Platform == Platform.both))
                    return;

                if (streamerSetPermissionLevel != null)
                {
                    if (streamerSetPermissionLevel <= commandArguments.ReachedPermissionLevel)
                    {
                        await command.ActionAsync(commandArguments);
                        return;
                    }
                    else
                    {
                        await commandArguments.Client.SendMessageAsync("You're not allowed to execute this comman.");
                        return;
                    }
                }
                else
                {
                    if (command.TwitchPermissionLevel <= commandArguments.ReachedPermissionLevel)
                    {
                        await command.ActionAsync(commandArguments);
                        return;
                    }
                    else
                    {
                        await commandArguments.Client.SendMessageAsync("You're not allowed to execute this command.");
                        return;
                    }
                }
            }
            else if (commandArguments.Platform == Platform.Discord)
            {
                if (!(command.Platform == Platform.Discord || command.Platform == Platform.both))
                    return;

                if (streamerSetPermissionLevel != null)
                {
                    if (streamerSetPermissionLevel <= commandArguments.ReachedPermissionLevel)
                    {
                        await command.ActionAsync(commandArguments);
                        return;
                    }
                    else
                    {
                        await commandArguments.Client.SendMessageAsync("You're not allowed to execute this command.");
                        return;
                    }
                }
                else
                {
                    if (command.DiscordPermissionLevel <= commandArguments.ReachedPermissionLevel)
                    {
                        await command.ActionAsync(commandArguments);
                        return;
                    }
                    else
                    {
                        await commandArguments.Client.SendMessageAsync("You're not allowed to execute this command.");
                        return;
                    }
                }
            }
        }

        private static PermissionLevel? GetStreamerSetPermissionLevel(string commandName, Streamer streamer, Platform platform)
        {
            PermissionLevel? streamerSetPermissionLevel = null;
            using (var db = new CuriosityContext())
            {
                var command = db.Commands
                    .Where(c => c.Streamer.Id == streamer.Id)
                    .SingleOrDefault(c => c.Name == commandName);

                if (command != null)
                {
                    if (command.DiscordPermissionLevel != null && platform == Platform.Discord)
                        streamerSetPermissionLevel = command.DiscordPermissionLevel;
                    else if (command.TwitchPermissionLevel != null && platform == Platform.Twitch)
                        streamerSetPermissionLevel = command.TwitchPermissionLevel;
                }
            }

            return streamerSetPermissionLevel;
        }
    }
}
