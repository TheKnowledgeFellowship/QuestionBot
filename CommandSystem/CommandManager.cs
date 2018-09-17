using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using QuestionBot.CommandSystem.SpecialCommands;
using QuestionBot.Models;
using TwitchLib.Client.Events;

namespace QuestionBot.CommandSystem
{
    public class CommandManager
    {
        public List<IStreamerCommand> StreamerCommands { get; set; } = new List<IStreamerCommand>();
        public List<ISpecialCommand> SpecialCommands { get; set; } = new List<ISpecialCommand>();

        public EventHandler<QuestionBotEnabledArgs> QuestionBotEnabledCommandSuccess;
        public EventHandler<RemoveStreamerArgs> RemoveStreamerCommandSuccess;

        public CommandManager()
        {
            // SpecialCommands:
            // admin-
            SpecialCommands.Add(new Admin());
            // admin-streamer-
            SpecialCommands.Add(new AdminStreamer());
            // admin-streamer-permit-
            SpecialCommands.Add(new AdminStreamerPermit());
            SpecialCommands.Add(new AdminStreamerPermitId());
            // admin-streamer-remove-
            SpecialCommands.Add(new AdminStreamerRemove());
            var adminStreamerRemoveId = new AdminStreamerRemoveId();
            adminStreamerRemoveId.RemoveStreamer += HandleRemoveStreamer;
            SpecialCommands.Add(adminStreamerRemoveId);
            // enable-
            SpecialCommands.Add(new Enable());
            var EnableNameId = new EnableNameId();
            EnableNameId.QuestionBotEnabled += HandleQuestionBotEnabled;
            SpecialCommands.Add(EnableNameId);
            var EnableNameName = new EnableNameName();
            EnableNameName.QuestionBotEnabled += HandleQuestionBotEnabled;
            SpecialCommands.Add(EnableNameName);
            // StreamerCommands:
            // config-
            StreamerCommands.Add(new StreamerCommands.Config());
            // config-commandpermissionlevel-
            StreamerCommands.Add(new StreamerCommands.ConfigCommandPermissionLevel());
            StreamerCommands.Add(new StreamerCommands.ConfigCommandPermissionLevelPrint());
            StreamerCommands.Add(new StreamerCommands.ConfigCommandPermissionLevelPrintEveryone());
            StreamerCommands.Add(new StreamerCommands.ConfigCommandPermissionLevelPrintModerator());
            StreamerCommands.Add(new StreamerCommands.ConfigCommandPermissionLevelPrintStreamer());
            StreamerCommands.Add(new StreamerCommands.ConfigCommandPermissionLevelSet());
            StreamerCommands.Add(new StreamerCommands.ConfigCommandPermissionLevelSetCommandPlatformPermissionLevel());
            // config-discordmoderator-
            StreamerCommands.Add(new StreamerCommands.ConfigDiscordModerator());
            StreamerCommands.Add(new StreamerCommands.ConfigDiscordModeratorAddId());
            StreamerCommands.Add(new StreamerCommands.ConfigDiscordModeratorPrint());
            StreamerCommands.Add(new StreamerCommands.ConfigDiscordModeratorRemoveId());
            // config-questionrecognitionmode-
            StreamerCommands.Add(new StreamerCommands.ConfigQuestionRecognitionMode());
            StreamerCommands.Add(new StreamerCommands.ConfigQuestionRecognitionModePrint());
            // config-questionrecognitionmode-set-
            StreamerCommands.Add(new StreamerCommands.ConfigQuestionRecognitionModeSet());
            StreamerCommands.Add(new StreamerCommands.ConfigQuestionRecognitionModeSetBoth());
            StreamerCommands.Add(new StreamerCommands.ConfigQuestionRecognitionModeSetByCommand());
            StreamerCommands.Add(new StreamerCommands.ConfigQuestionRecognitionModeSetByKeywords());
            StreamerCommands.Add(new StreamerCommands.ConfigQuestionRecognitionModeSetDefault());
            // config-twitchmoderator-
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchModerator());
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchModeratorDisable());
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchModeratorEnable());
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchModeratorPrint());
            // config-twitchquestioncommandprefix-
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchQuestionCommandPrefix());
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchQuestionCommandPrefixPrint());
            // config-twitchquestioncommandprefix-set-
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchQuestionCommandPrefixSet());
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchQuestionCommandPrefixSetChar());
            StreamerCommands.Add(new StreamerCommands.ConfigTwitchQuestionCommandPrefixSetDefault());
            // question-
            StreamerCommands.Add(new StreamerCommands.Question());
            // question-answered-
            StreamerCommands.Add(new StreamerCommands.QuestionAnswered());
            StreamerCommands.Add(new StreamerCommands.QuestionAnsweredAll());
            StreamerCommands.Add(new StreamerCommands.QuestionAnsweredId());
            // question-print-
            StreamerCommands.Add(new StreamerCommands.QuestionPrint());
            StreamerCommands.Add(new StreamerCommands.QuestionPrintAll());
            StreamerCommands.Add(new StreamerCommands.QuestionPrintAnswered());
            StreamerCommands.Add(new StreamerCommands.QuestionPrintId());
            StreamerCommands.Add(new StreamerCommands.QuestionPrintUnanswered());
            // question-remove-
            StreamerCommands.Add(new StreamerCommands.QuestionRemove());
            StreamerCommands.Add(new StreamerCommands.QuestionRemoveAll());
            StreamerCommands.Add(new StreamerCommands.QuestionRemoveAnswered());
            StreamerCommands.Add(new StreamerCommands.QuestionRemoveId());
            StreamerCommands.Add(new StreamerCommands.QuestionRemoveUnanswered());
            // question-unanswered-
            StreamerCommands.Add(new StreamerCommands.QuestionUnanswered());
            StreamerCommands.Add(new StreamerCommands.QuestionUnansweredAll());
            StreamerCommands.Add(new StreamerCommands.QuestionUnansweredId());
        }

        public async Task HandleCommandAsync(string commandText, MessageCreateEventArgs args, Discord.Client client)
        {
            // Check for ISpecialCommand (these are Discord only).
            if (await HandleSpecialCommandAsync(commandText, args, client))
                return;
            // Check for IStreamerCommand.
            IStreamerCommand targetedCommand = GetStreamerCommand(commandText);
            if (targetedCommand == null)
                return;

            Logger.Console.LogCommand(targetedCommand.Name, args);
            var platformClient = new CommandSystem.PlatformClients.DiscordClient(args, client);

            var messageByStreamer = false;
            var messageByModerator = false;
            Streamer streamer;
            using (var db = new CuriosityContext())
            {
                messageByStreamer = db.Streamer
                    .Any(s => s.DiscordId == args.Author.Id);
                streamer = db.Streamer
                    .SingleOrDefault(s => s.DiscordGuild == args.Guild.Id);
                if (streamer != null)
                {
                    messageByModerator = db.Moderators
                        .Where(m => m.Streamer.Id == streamer.Id)
                        .Any(m => m.DiscordId == args.Author.Id);
                }
            }

            if (streamer == null)
                return;

            var reachedPermissionLevel = PermissionLevel.everyone;
            if (messageByModerator)
                reachedPermissionLevel = PermissionLevel.Moderator;
            if (messageByStreamer)
                reachedPermissionLevel = PermissionLevel.Streamer;

            var commandArguments = new StreamerCommandArguments(commandText, platformClient, streamer, reachedPermissionLevel, Platform.Discord);
            await CommandShell.ExecuteStreamerCommandAsync(commandArguments, targetedCommand);
        }

        public async Task HandleCommandAsync(string commandText, OnMessageReceivedArgs args, Twitch.Client client)
        {
            // Check for IStreamerCommand.
            IStreamerCommand targetedCommand = GetStreamerCommand(commandText);
            if (targetedCommand == null)
                return;

            Logger.Console.LogCommand(targetedCommand.Name, args);
            var platformClient = new CommandSystem.PlatformClients.TwitchClient(client);

            Streamer streamer;
            using (var db = new CuriosityContext())
                streamer = db.Streamer
                    .SingleOrDefault(s => s.TwitchChannelName == args.ChatMessage.Channel);

            var reachedPermissionLevel = PermissionLevel.everyone;
            if (streamer.TwitchModeratorEnabled && args.ChatMessage.IsModerator)
                reachedPermissionLevel = PermissionLevel.Moderator;
            if (args.ChatMessage.IsBroadcaster)
                reachedPermissionLevel = PermissionLevel.Streamer;

            var commandArguments = new StreamerCommandArguments(commandText, platformClient, streamer, reachedPermissionLevel, Platform.Twitch);
            await CommandShell.ExecuteStreamerCommandAsync(commandArguments, targetedCommand);
        }

        protected virtual void OnRemoveStreamerCommandSuccess(ulong discordId) => RemoveStreamerCommandSuccess?.Invoke(this, new RemoveStreamerArgs(discordId));
        protected virtual void OnQuestionBotEnabledCommandSuccess(Streamer streamer) => QuestionBotEnabledCommandSuccess?.Invoke(this, new QuestionBotEnabledArgs(streamer));

        private void HandleRemoveStreamer(object sender, RemoveStreamerArgs args) => OnRemoveStreamerCommandSuccess(args.DiscordId);
        private void HandleQuestionBotEnabled(object sender, QuestionBotEnabledArgs args) => OnQuestionBotEnabledCommandSuccess(args.Streamer);

        private IStreamerCommand GetStreamerCommand(string commandText)
        {
            IStreamerCommand streamerCommand = null;
            foreach (var command in StreamerCommands)
            {
                if (Regex.IsMatch(commandText, command.Call, RegexOptions.IgnoreCase))
                {
                    if (streamerCommand != null)
                    {
                        if (streamerCommand.Name.Length < command.Name.Length)
                            streamerCommand = command;
                    }
                    else
                        streamerCommand = command;
                }
            }
            return streamerCommand;
        }
        // Discord only.
        private async Task<bool> HandleSpecialCommandAsync(string commandText, MessageCreateEventArgs messageArgs, Discord.Client client)
        {
            ISpecialCommand targetedSpecialCommand = null;
            foreach (var command in SpecialCommands)
            {
                if (Regex.IsMatch(commandText, command.Call, RegexOptions.IgnoreCase))
                {
                    if (targetedSpecialCommand != null)
                    {
                        if (targetedSpecialCommand.Name.Length < command.Name.Length)
                            targetedSpecialCommand = command;
                    }
                    else
                        targetedSpecialCommand = command;
                }
            }

            if (targetedSpecialCommand == null)
                return false;

            Logger.Console.LogCommand(targetedSpecialCommand.Name, messageArgs);
            var platformClient = new PlatformClients.DiscordClient(messageArgs, client);
            // Handle admin commands.
            if (targetedSpecialCommand.Name.StartsWith("admin"))
            {
                var config = Config.Config.Load();
                if (config.Discord.Admin == messageArgs.Author.Id)
                {
                    var commandArguments = new SpecialCommandArguments(commandText, platformClient, Platform.Discord, messageArgs);
                    await targetedSpecialCommand.ActionAsync(commandArguments);
                }
                else
                    await platformClient.SendMessageAsync("You aren't allowed to execute this command.");
            }
            // Handle enable command.
            if (targetedSpecialCommand.Name.StartsWith("enable"))
            {
                var config = Config.Config.Load();
                PermittedStreamer permittedStreamer;
                using (var db = new CuriosityContext())
                {
                    permittedStreamer = db.PermittedStreamers
                        .SingleOrDefault(ps => ps.DiscordId == messageArgs.Author.Id);
                }
                if (permittedStreamer != null)
                {
                    var commandArguments = new SpecialCommandArguments(commandText, platformClient, Platform.Discord, messageArgs);
                    await targetedSpecialCommand.ActionAsync(commandArguments);
                }
                else
                    await platformClient.SendMessageAsync("You can't execute this command, sicne you're not a permitted streamer. Contact the admin for help.");
            }

            return true;
        }
    }
}
