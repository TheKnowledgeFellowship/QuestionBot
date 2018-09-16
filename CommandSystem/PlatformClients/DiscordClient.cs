using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace QuestionBot.CommandSystem.PlatformClients
{
    public class DiscordClient : IPlatformClient
    {
        private MessageCreateEventArgs _args;
        private Discord.Client _client;

        public DiscordClient(MessageCreateEventArgs args, Discord.Client client)
        {
            _args = args;
            _client = client;
        }

        public async Task SendMessageAsync(string content) => await _client.SendMessageAsync(_args.Channel.Id, content);
    }
}
