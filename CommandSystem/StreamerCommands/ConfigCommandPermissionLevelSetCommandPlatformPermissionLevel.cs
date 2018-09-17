using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using project.CommandSystem;
using QuestionBot.Models;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigCommandPermissionLevelSetCommandPlatformPermissionLevel : IStreamerCommand
    {
        private List<string> _commandsModerator = new List<string>();
        private List<string> _commandsEveryone = new List<string>();

        public string Name => "config-commandpermissionlevel-set-command-platform-permissionlevel";
        public string Call => @"^config\s+commandpermissionlevel\s+set\s+.*\s+(twitch|discord)\s+(streamer|moderator|everyone)\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.both;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^config\s+commandpermissionlevel\s+set\s+", RegexOptions.IgnoreCase);
            var commandText = commandArguments.CommandText.Remove(match.Index, match.Length).Trim();

            match = Regex.Match(commandText, @"(twitch|discord)\s+(streamer|moderator|everyone)\s*", RegexOptions.IgnoreCase);
            System.Console.WriteLine($"{match.Index} {match.Length}");

            var argumentCommand = commandText.Substring(0, match.Index).Trim().ToLower();
            argumentCommand = Regex.Replace(argumentCommand, @"\s+", "-").Trim();
            commandText = commandText.Remove(0, match.Index).Trim();
            var arguments = commandText.Split(" ");

            var argumentPlatform = arguments[0];
            commandText = commandText.Remove(0, argumentPlatform.Length).Trim();
            arguments = commandText.Split(" ");

            var argumentPermissionLevel = arguments[0];

            Platform platform;
            if (argumentPlatform.ToLower().Trim() == "twitch")
                platform = Platform.Twitch;
            else
                platform = Platform.Discord;

            PermissionLevel permissionLevel;
            if (argumentPermissionLevel.ToLower().Trim() == "everyone")
                permissionLevel = PermissionLevel.everyone;
            else if (argumentPermissionLevel.ToLower().Trim() == "moderator")
                permissionLevel = PermissionLevel.Moderator;
            else
                permissionLevel = PermissionLevel.Streamer;

            if (permissionLevel == PermissionLevel.everyone)
            {
                string targetCommand = null;
                foreach (var allowedCommand in CommandOverview.CommandsEveryone)
                {
                    if (allowedCommand.Item1 == argumentCommand)
                        targetCommand = allowedCommand.Item1;
                }
                if (targetCommand == null)
                {
                    await commandArguments.Client.SendMessageAsync("Sorry, the command you specified isn't a command, that can be potentially accessed by everyone.");
                    return;
                }
                await SetPermissionsInDatabase(argumentCommand, CommandOverview.CommandsEveryone, commandArguments.Streamer.Id, platform, permissionLevel);
                await commandArguments.Client.SendMessageAsync($"You successfully set the permission level of {targetCommand} to {permissionLevel} for {platform}.");
            }
            else
            {
                string targetCommand = null;
                foreach (var allowedCommand in CommandOverview.CommandsModerator)
                {
                    if (allowedCommand.Item1 == argumentCommand)
                        targetCommand = allowedCommand.Item1;
                }
                if (targetCommand == null)
                {
                    await commandArguments.Client.SendMessageAsync("Sorry, the command you specified isn't a command, where the permission level can be changed.");
                    return;
                }
                await SetPermissionsInDatabase(argumentCommand, CommandOverview.CommandsModerator, commandArguments.Streamer.Id, platform, permissionLevel);
                await commandArguments.Client.SendMessageAsync($"You successfully set the permission level of {targetCommand} to {permissionLevel} for {platform}.");
            }
        }

        private async Task SetPermissionsInDatabase(string commandName, List<(string, PermissionLevel)> commandNames, int streamerId, Platform platform, PermissionLevel permissionLevel)
        {
            using (var db = new CuriosityContext())
            {
                foreach (var command in commandNames)
                {
                    if (command.Item1.StartsWith(commandName))
                    {
                        var potentialCommand = db.Commands
                            .Where(c => c.Streamer.Id == streamerId)
                            .SingleOrDefault(c => c.Name == command.Item1);
                        if (potentialCommand != null)
                        {
                            if (platform == Platform.Twitch)
                                potentialCommand.TwitchPermissionLevel = permissionLevel;
                            else
                                potentialCommand.DiscordPermissionLevel = permissionLevel;
                        }
                        else
                        {
                            if (platform == Platform.Twitch)
                                await db.Commands.AddAsync(new Command(command.Item1, streamerId)
                                {
                                    TwitchPermissionLevel = permissionLevel
                                });
                            else
                                await db.Commands.AddAsync(new Command(command.Item1, streamerId)
                                {
                                    DiscordPermissionLevel = permissionLevel
                                });
                        }
                    }
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
