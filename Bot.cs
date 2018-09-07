using System.Threading.Tasks;
using QuestionBot.ItemsJson;
using QuestionBot.Models;

namespace QuestionBot
{
    public class Bot
    {
        private DiscordClient _discordClient;
        private ItemsJson<Streamer> _streamer;

        public Bot()
        {
            _discordClient = new DiscordClient();
            _streamer = new ItemsJson<Streamer>("Streamer.json");
        }

        public async Task StartAsync()
        {
            await _discordClient.ConnectAsync();
        }
    }
}
