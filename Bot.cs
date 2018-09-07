using System.Threading.Tasks;
using QuestionBot.ItemsJson;
using QuestionBot.Models;

namespace QuestionBot
{
    public class Bot
    {
        private Discord.Client _discordClient;
        private ItemsJson<Streamer> _streamer;

        public Bot()
        {
            _streamer = new ItemsJson<Streamer>("Streamer.json");
            _discordClient = new Discord.Client(_streamer);
        }

        public async Task StartAsync()
        {
            await _discordClient.ConnectAsync();
        }
    }
}
