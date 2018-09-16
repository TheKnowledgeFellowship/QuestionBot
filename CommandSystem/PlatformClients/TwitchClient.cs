using System.Threading.Tasks;

namespace QuestionBot.CommandSystem.PlatformClients
{
    public class TwitchClient : IPlatformClient
    {
        private Twitch.Client _client;

        public TwitchClient(Twitch.Client client)
        {
            _client = client;
        }

        public Task SendMessageAsync(string content)
        {
            _client.SendMessage(content);
            return Task.CompletedTask;
        }
    }
}
