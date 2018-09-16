using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuestionBot.Models;

namespace QuestionBot.CommandSystem.SpecialCommands
{
    public class RemoveStreamerArgs
    {
        public ulong DiscordId { get; set; }

        public RemoveStreamerArgs(ulong discordId) => this.DiscordId = discordId;
    }

    public class AdminStreamerRemoveId : ISpecialCommand
    {
        public string Name => "admin-streamer-remove-id";
        public string Call => @"^admin\s*streamer\s*remove\s*\d+";
        public Platform Platform => Platform.Discord;
        public EventHandler<RemoveStreamerArgs> RemoveStreamer;

        public async Task ActionAsync(SpecialCommandArguments commandArguments)
        {
            var match = Regex.Match(commandArguments.CommandText, @"^admin\s*streamer\s*remove\s*", RegexOptions.IgnoreCase);
            var arguments = commandArguments.CommandText.Remove(match.Index, match.Length).Trim().Split(" ");

            ulong discordId;
            if (!ulong.TryParse(arguments[0], out discordId))
            {
                await commandArguments.Client.SendMessageAsync($"The argument you provided could not be recognized.");
                return;
            }

            OnRemoveStreamer(discordId);
            await commandArguments.Client.SendMessageAsync($"The streamer with the discord id `{discordId}` has been removed from the permitted streamers, if they were part of them. All connected information got removed as well. The streamer is no longer able to use QuestionBot as a streamer.");
        }

        protected virtual void OnRemoveStreamer(ulong discordId) => RemoveStreamer?.Invoke(this, new RemoveStreamerArgs(discordId));
    }
}
