using System;
using System.Timers;
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

        public Client(string channelName, Streamer streamer)
        {
            var config = Config.Config.Load();
            _channelName = channelName;
            _streamer = streamer;
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
            _client.OnMessageReceived += HandleMessageReceived;

            _client.Connect();

            var reconnect = new Timer(TimeSpan.FromHours(6).TotalMilliseconds);
            reconnect.Elapsed += (Object source, ElapsedEventArgs e) => _client.Reconnect();
            reconnect.AutoReset = true;
            reconnect.Enabled = true;
        }

        public void Disconnect() => _client.Disconnect();

        protected virtual void OnQuestionReceived(Question question) => QuestionReceived?.Invoke(this, new QuestionReceivedArgs(question));

        private void SendMessage(string content)
        {
            _client.SendMessage(_channelName, content);
        }

        private void HandleMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message.ToLower();
            var isQuestion = false;

            if ((message.Contains("?")
            || message.Contains("question")
            || message.Contains("❓")
            || message.Contains("❔")
            || message.Contains("⁉"))
            && message.Contains(_channelName.ToLower()))
                isQuestion = true;

            if (message.Contains("!q"))
                isQuestion = true;

            if (isQuestion)
            {
                OnQuestionReceived(new Question(_streamer.DiscordId, message, e.ChatMessage.DisplayName, DateTime.Now));
                SendMessage($"@{e.ChatMessage.DisplayName} Your question got recognized.");
            }
        }
    }
}
