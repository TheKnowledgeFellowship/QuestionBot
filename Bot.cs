using System.Threading.Tasks;

namespace QuestionBot
{
    class Bot
    {
        private DiscordClient _discordClient;

        public Bot()
        {
            _discordClient = new DiscordClient();
        }

        public async Task StartAsync()
        {
            await _discordClient.ConnectAsync();
        }
    }
}
