using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuestionBot.Models;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class EnableNameName : ISpecialCommand
    {
        public string Name => "enable-name-name";
        public string Call => @"^enable\s+.+\s+.+";
        public Platform Platform => Platform.Discord;
        public EventHandler<QuestionBotEnabledArgs> QuestionBotEnabled;

        public async Task ActionAsync(SpecialCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^enable\s+", RegexOptions.IgnoreCase);
            var arguments = commandArguments.CommandText.Remove(match.Index, match.Length).Trim().Split(" ");
            var firstArgument = arguments[0];

            arguments = commandArguments.CommandText.Remove(match.Index, match.Length).Trim().Remove(0, firstArgument.Length).Trim().Split();
            var secondArgument = arguments[0];

            var discordChannel = commandArguments.MessageArgs.Guild.Channels
                .SingleOrDefault(c => c.Name.ToLower().Trim() == secondArgument.ToLower().Trim());

            if (discordChannel == null)
            {
                await commandArguments.Client.SendMessageAsync("The provided channel name could not be read.");
                return;
            }

            var discordChannelId = discordChannel.Id;

            Streamer potentialStreamer;
            using (var db = new CuriosityContext())
            {
                potentialStreamer = db.Streamer
                    .SingleOrDefault(s => s.DiscordId == commandArguments.MessageArgs.Author.Id);
            }
            if (potentialStreamer != null)
            {
                await commandArguments.Client.SendMessageAsync($"You already enabled QuestionBot with the following values: [ TwitchChannelName: {potentialStreamer.TwitchChannelName} ] [ DiscordChannel: {potentialStreamer.DiscordChannel} ]");
                return;
            }

            var streamer = new Streamer(commandArguments.MessageArgs.Author.Id, discordChannelId, commandArguments.MessageArgs.Guild.Id, firstArgument);
            OnQuestionBotEnabled(streamer);
            await commandArguments.Client.SendMessageAsync($"You successfully enabled QuestionBot with the following values: [ TwitchChannelName: {streamer.TwitchChannelName} ] [ DiscordChannel: {streamer.DiscordChannel} ]");
        }

        protected virtual void OnQuestionBotEnabled(Streamer streamer) => QuestionBotEnabled?.Invoke(this, new QuestionBotEnabledArgs(streamer));
    }
}
