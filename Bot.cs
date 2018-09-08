using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionBot.ItemsJson;
using QuestionBot.Models;

namespace QuestionBot
{
    public class Bot
    {
        private Discord.Client _discordClient;
        private ItemsJson<Streamer> _streamer;
        private List<Twitch.Client> _twitchClients = new List<Twitch.Client>();
        private Dictionary<ulong, ItemsJson<Question>> _questions = new Dictionary<ulong, ItemsJson<Question>>();

        public Bot()
        {
            _streamer = new ItemsJson<Streamer>("Streamer.json");
            _discordClient = new Discord.Client(_streamer, _questions);

            _discordClient.CommandsEvents.QuestionBotEnabled += HandleQuestionBotEnabled;
        }

        public async Task StartAsync()
        {
            await _discordClient.ConnectAsync();
            foreach (var streamer in _streamer.Items)
            {
                StreamerInit(streamer);
            }
        }

        private void StreamerInit(Streamer streamer)
        {
            var client = new Twitch.Client(streamer.TwitchChannelName, streamer);
            client.Connect();
            client.QuestionReceived += HandleQuestionReceivedAsync;
            _twitchClients.Add(client);

            var questions = new ItemsJson<Question>($"Questions{streamer.DiscordId}.json");
            _questions.Add(streamer.DiscordId, questions);
        }

        private async void HandleQuestionReceivedAsync(object sender, Twitch.QuestionReceivedArgs e)
        {
            var question = e.Question;
            var streamer = _streamer.Items.Single(s => s.DiscordId == question.StreamerId);

            // Clean the message from the @streamer or streamer.
            question.Content = question.Content.Replace($"@{streamer.TwitchChannelName} ", "");
            question.Content = question.Content.Replace($"{streamer.TwitchChannelName} ", "");

            question.Content = question.Content.Trim();

            var questionId = await _questions[streamer.DiscordId].AddItemAsync(question);
            question.Id = questionId;
            await _discordClient.SendMessageAsync(streamer.DiscordChannel, question.ToString());
        }

        private async void HandleQuestionBotEnabled(object sender, Discord.QuestionBotEnabledArgs e) => StreamerInit(e.Streamer);
    }
}
