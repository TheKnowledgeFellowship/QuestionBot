using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using QuestionBot.CommandSystem;
using QuestionBot.Models;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace QuestionBot.Twitch
{
    public class QuestionReceivedArgs
    {
        public Question Question { get; set; }

        public QuestionReceivedArgs(Question question) => Question = question;
    }

    public class Client
    {
        private readonly ConnectionCredentials _credentials;
        private string _channelName;
        private TwitchClient _client;
        private Streamer _streamer;
        private CommandManager _commandManager;

        public Client(string channelName, Streamer streamer, CommandManager commandManager)
        {
            var config = Config.Config.Load();
            _channelName = channelName;
            _streamer = streamer;
            _commandManager = commandManager;
            _credentials = new ConnectionCredentials(
                twitchUsername: config.Twitch.Username,
                twitchOAuth: config.Twitch.AccessToken
            );
       }

        public event EventHandler<QuestionReceivedArgs> QuestionReceived;

        public void Connect()
        {
            _client = new TwitchClient();
            _client.Initialize(_credentials, _channelName);
            _client.OnMessageReceived += HandleMessageReceivedAsync;
            _client.OnConnected += (Object source, OnConnectedArgs e) => Logger.Console.Log(Logger.Category.Twitch, $"({_streamer.TwitchChannelName}) Connected.");
            _client.OnDisconnected += (Object source, OnDisconnectedArgs e) => Logger.Console.Log(Logger.Category.Twitch, $"({_streamer.TwitchChannelName}) Disconnected.", true);
            _client.OnConnectionError += (Object source, OnConnectionErrorArgs e) => Logger.Console.Log(Logger.Category.Twitch, $"({_streamer.TwitchChannelName}) Connection Error.", true);

            _client.Connect();

            var reconnect = new Timer(TimeSpan.FromHours(6).TotalMilliseconds);
            reconnect.Elapsed += (Object source, ElapsedEventArgs e) =>
            {
                _client.Reconnect();
                Logger.Console.Log(Logger.Category.Twitch, $"({_streamer.TwitchChannelName}) Reconnecting.");
            };
            reconnect.AutoReset = true;
            reconnect.Enabled = true;
        }

        public void SendMessage(string content)
        {
            _client.SendMessage(_channelName, content);
            Logger.Console.Log(Logger.Category.Twitch, $"({_streamer.TwitchChannelName}) Message sent: {content}");
        }

        public void Disconnect() => _client.Disconnect();

        protected virtual void OnQuestionReceived(Question question) => QuestionReceived?.Invoke(this, new QuestionReceivedArgs(question));

        private async void HandleMessageReceivedAsync(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message.ToLower().Trim();
            Streamer streamer;
            using (var db = new CuriosityContext())
                streamer = db.Streamer
                    .SingleOrDefault(s => s.Id == _streamer.Id);
            if (streamer == null)
                return;
            var byKeywords = (streamer.QuestionRecognitionMode == QuestionRecognitionMode.ByKeywords || streamer.QuestionRecognitionMode == QuestionRecognitionMode.Both);
            var byCommand = (streamer.QuestionRecognitionMode == QuestionRecognitionMode.ByCommand || streamer.QuestionRecognitionMode == QuestionRecognitionMode.Both);

            var isQuestion = false;

            if ((message.Contains("?")
            || message.Contains("question")
            || message.Contains("❓")
            || message.Contains("❔")
            || message.Contains("⁉"))
            && message.Contains(_channelName.ToLower())
            && byKeywords)
                isQuestion = true;

            var commandPrefix = streamer.TwitchCommandPrefix;
            if ((message.Contains($"{commandPrefix}q ") || message.Contains($"{commandPrefix}question ")) && byCommand)
                isQuestion = true;

            if (isQuestion)
            {
                OnQuestionReceived(new Question(e.ChatMessage.Message, e.ChatMessage.DisplayName, DateTime.Now, _streamer.Id));
                Logger.Console.Log(Logger.Category.Twitch, $"({_streamer.TwitchChannelName}) Question received: \"{e.ChatMessage.Message}\"");
                SendMessage($"@{e.ChatMessage.DisplayName} your question got recognized.");
                return;
            }

            if (message.StartsWith("!questionbot"))
            {
                var commandText = e.ChatMessage.Message.Remove(0, 12).Trim();
                await _commandManager.HandleCommandAsync(commandText, e, this);
            }
        }
    }
}
