using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionBot.CommandSystem;
using QuestionBot.CommandSystem.SpecialCommands;
using QuestionBot.Models;

namespace QuestionBot
{
    public class Bot
    {
        private CommandManager _commandManager;
        private Discord.Client _discordClient;
        private Twitch.Api _twitchApi;
        private Dictionary<ulong, Twitch.Client> _twitchClients = new Dictionary<ulong, Twitch.Client>();

        public Bot()
        {
            _commandManager = new CommandManager();
            _discordClient = new Discord.Client(_commandManager);
            _twitchApi = new Twitch.Api();

            _commandManager.QuestionBotEnabledCommandSuccess += HandleQuestionBotEnabledCommandSuccess;
            _commandManager.RemoveStreamerCommandSuccess += HandleRemoveStreamerCommandSuccess;
        }

        public async Task StartAsync()
        {
            await _discordClient.ConnectAsync();

            List<Streamer> allStreamer;
            using (var db = new CuriosityContext())
                allStreamer = db.Streamer.ToList();

            foreach (var streamer in allStreamer)
                StreamerInit(streamer);
        }

        private async Task StreamerCreateAsync(Streamer streamer)
        {
            streamer.TwitchClientId = await _twitchApi.GetChannelIdFromChannelName(streamer.TwitchChannelName);

            var client = new Twitch.Client(streamer.TwitchChannelName, streamer, _commandManager);
            client.Connect();
            client.QuestionReceived += HandleQuestionReceivedAsync;
            _twitchClients.Add(streamer.DiscordId, client);

            using (var db = new CuriosityContext())
            {
                await db.Streamer.AddAsync(streamer);
                await db.SaveChangesAsync();
            }
        }

        private void StreamerInit(Streamer streamer)
        {
            var client = new Twitch.Client(streamer.TwitchChannelName, streamer, _commandManager);
            client.Connect();
            client.QuestionReceived += HandleQuestionReceivedAsync;
            _twitchClients.Add(streamer.DiscordId, client);
        }

        private async void HandleQuestionReceivedAsync(object sender, Twitch.QuestionReceivedArgs e)
        {
            var question = e.Question;
            Streamer streamer;
            using (var db = new CuriosityContext())
                streamer = db.Streamer.SingleOrDefault(s => s.Id == question.StreamerId);

            // Clean the message from the @streamer or streamer.
            // question.Content = question.Content.Replace($"@{streamer.TwitchChannelName} ", "");
            // question.Content = question.Content.Replace($"{streamer.TwitchChannelName} ", "");

            question.Content = question.Content.Trim();
            question.WhileLive = await _twitchApi.CheckStreamerOnlineStatus(streamer.TwitchClientId);

            using (var db = new CuriosityContext())
            {
                var lastQuestion = db.Questions
                    .Where(q => q.Streamer.Id == streamer.Id)
                    .LastOrDefault();

                if (lastQuestion != null)
                    question.ReadableId = lastQuestion.ReadableId + 1;
                else
                    question.ReadableId = 1;

                await db.Questions.AddAsync(question);
                await db.SaveChangesAsync();
            }
            await _discordClient.SendMessageAsync(streamer.DiscordChannel, question.ToLightMarkdownString());
        }

        private async void HandleQuestionBotEnabledCommandSuccess(object sender, QuestionBotEnabledArgs e) => await StreamerCreateAsync(e.Streamer);

        private async void HandleRemoveStreamerCommandSuccess(object sender, RemoveStreamerArgs e)
        {
            var discordId = e.DiscordId;

            if (_twitchClients.Keys.Contains(discordId))
            {
                _twitchClients[discordId].Disconnect();
                _twitchClients.Remove(discordId);
            }

            using (var db = new CuriosityContext())
            {
                var permittedStreamer = db.PermittedStreamers
                    .SingleOrDefault(ps => ps.DiscordId == discordId);
                if (permittedStreamer != null)
                    db.PermittedStreamers.Remove(permittedStreamer);

                var streamer = db.Streamer
                    .SingleOrDefault(ps => ps.DiscordId == discordId);
                if (streamer != null)
                    db.Streamer.Remove(streamer);

                await db.SaveChangesAsync();
            }
        }
    }
}
