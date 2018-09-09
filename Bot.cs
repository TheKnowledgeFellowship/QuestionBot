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
        private Twitch.Api _twitchApi;
        private List<Twitch.Client> _twitchClients = new List<Twitch.Client>();
        private Dictionary<ulong, ItemsJson<Question>> _questions = new Dictionary<ulong, ItemsJson<Question>>();

        public Bot()
        {
            _twitchApi = new Twitch.Api();
            _streamer = new ItemsJson<Streamer>("Streamer.json");
            _discordClient = new Discord.Client(_streamer, _questions);

            _discordClient.CommandsEvents.QuestionBotEnabled += HandleQuestionBotEnabled;
            _discordClient.CommandsEvents.RemoveStreamer += HandleRemoveStreamer;
        }

        public async Task StartAsync()
        {
            await _discordClient.ConnectAsync();
            foreach (var streamer in _streamer.Items)
            {
                await StreamerInitAsync(streamer);
            }
        }

        private async Task StreamerInitAsync(Streamer streamer, bool creation = false)
        {
            // One time first time actions.
            if (creation)
                streamer.TwitchClientId = await _twitchApi.GetChannelIdFromChannelName(streamer.TwitchChannelName);

            var client = new Twitch.Client(streamer.TwitchChannelName, streamer);
            client.Connect();
            client.QuestionReceived += HandleQuestionReceivedAsync;
            _twitchClients.Add(client);

            var questions = new ItemsJson<Question>($"Questions{streamer.DiscordId}.json");
            _questions.Add(streamer.DiscordId, questions);

            if (creation)
                await _streamer.AddItemAsync(streamer);
        }

        private async void HandleQuestionReceivedAsync(object sender, Twitch.QuestionReceivedArgs e)
        {
            var question = e.Question;
            var streamer = _streamer.Items.Single(s => s.DiscordId == question.StreamerId);

            // Clean the message from the @streamer or streamer.
            // question.Content = question.Content.Replace($"@{streamer.TwitchChannelName} ", "");
            // question.Content = question.Content.Replace($"{streamer.TwitchChannelName} ", "");

            question.Content = question.Content.Trim();
            question.WhileLive = await _twitchApi.CheckStreamerOnlineStatus(streamer.TwitchClientId);

            var questionId = await _questions[streamer.DiscordId].AddItemAsync(question);
            question.Id = questionId;
            await _discordClient.SendMessageAsync(streamer.DiscordChannel, question.ToLightMarkdownString());
        }

        private async void HandleQuestionBotEnabled(object sender, Discord.QuestionBotEnabledArgs e) => await StreamerInitAsync(e.Streamer, true);

        private async void HandleRemoveStreamer(object sender, Discord.RemoveStreamerArgs e)
        {
            var id = e.StreamerId;

            var storedStreamer = _streamer.Items.SingleOrDefault(s => s.DiscordId == id);
            if (storedStreamer != null)
                await _streamer.RemoveItemAsync(storedStreamer.Id);

            _questions[id].Purge();
            _questions.Remove(id);
        }
    }
}
