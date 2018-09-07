using System.Threading.Tasks;
using DSharpPlus;

namespace QuestionBot
{
    public class DiscordClient
    {
        private DSharpPlus.DiscordClient _discord;

        public DiscordClient()
        {
            var config = Config.Config.Load();
            _discord = new DSharpPlus.DiscordClient(new DiscordConfiguration()
            {
                Token = config.Discord.Token,
                TokenType = TokenType.Bot
            });
        }

        public async Task ConnectAsync()
        {
            await _discord.ConnectAsync();
        }

        public async Task DisconnectAsync()
        {
            await _discord.DisconnectAsync();
        }

        public async Task SendMessageAsync(ulong channelId, string content)
        {
            var channel = await _discord.GetChannelAsync(channelId);
            await _discord.SendMessageAsync(channel, content);
        }
    }
}
