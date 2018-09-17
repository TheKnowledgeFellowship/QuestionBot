using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.CommandSystem;
using QuestionBot.Models;

namespace QuestionBot.CommandSystem.StreamerCommands
{
    public class ConfigCommandPermissionLevelPrintModerator : IStreamerCommand
    {
        public string Name => "config-commandpermissionlevel-print-moderator";
        public string Call => @"^config\s+commandpermissionlevel\s+print\s+moderator\s*";
        public PermissionLevel TwitchPermissionLevel => PermissionLevel.Streamer;
        public PermissionLevel DiscordPermissionLevel => PermissionLevel.Streamer;
        public Platform Platform => Platform.Discord;

        public async Task ActionAsync(StreamerCommandArguments commandArguments)
        {
            List<Command> storedCommands;
            using (var db = new CuriosityContext())
            {
                storedCommands = db.Commands
                    .Where(c => c.Streamer.Id == commandArguments.Streamer.Id)
                    .ToList();
            }

            var response = "**OVERVIEW - ALLOWED: STREAMER, MODERATOR**\n\n";
            response += "**[ Command ] [ Current Discord Permission Level / Current Twitch Permission Level ] [ Default Permission Level ]**\n";

            char? currentDiscordPermissionLevel;
            char? currentTwitchPermissionLevel;
            char defaultPermissionLevel;
            for (int i = 0; i < CommandOverview.CommandsModerator.Count; i++)
            {
                currentDiscordPermissionLevel = null;
                currentTwitchPermissionLevel = null;
                defaultPermissionLevel = PermissionLevelToChar(CommandOverview.CommandsStreamer[i].Item2);

                var storedCommand = storedCommands
                    .SingleOrDefault(sc => sc.Name == CommandOverview.CommandsStreamer[i].Item1);
                if (storedCommand != null)
                {
                    if (storedCommand.DiscordPermissionLevel.HasValue)
                        currentDiscordPermissionLevel = PermissionLevelToChar(storedCommand.DiscordPermissionLevel.Value);
                    if (storedCommand.TwitchPermissionLevel.HasValue)
                        currentTwitchPermissionLevel = PermissionLevelToChar(storedCommand.TwitchPermissionLevel.Value);
                }

                if (currentDiscordPermissionLevel == null)
                    currentDiscordPermissionLevel = defaultPermissionLevel;
                if (currentTwitchPermissionLevel == null)
                    currentTwitchPermissionLevel = defaultPermissionLevel;

                response += $"[ {CommandOverview.CommandsStreamer[i].Item1} ] [ {currentDiscordPermissionLevel} / {currentDiscordPermissionLevel} ] [ {defaultPermissionLevel} ]\n";

                // Split the message in multiple messages to not go over Discords limit.
                if ((i + 1) % 20 == 0)
                {
                    await commandArguments.Client.SendMessageAsync(response);
                    response = "";
                }
            }

            if (response != "")
                await commandArguments.Client.SendMessageAsync(response);
        }

        private char PermissionLevelToChar(PermissionLevel permissionLevel)
        {
            switch (permissionLevel)
            {
                case (PermissionLevel.Streamer):
                    return 'S';
                case (PermissionLevel.Moderator):
                    return 'M';
                case (PermissionLevel.everyone):
                    return 'E';
                default:
                    return '?';
            }
        }
    }
}
