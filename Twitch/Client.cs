using System;
using System.Linq;
using System.Threading;
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

            var reconnect = new System.Timers.Timer(TimeSpan.FromHours(6).TotalMilliseconds);
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
            // Check, if the client is connected to any channel and reconnect, if it's not.
            if (_client.JoinedChannels.Count == 0)
            {
                Logger.Console.Log(Logger.Category.Twitch, "The client is currently not connected to any channel. Therefore reconnect and wait until a new connection is in place.", true);
                _client.Reconnect();

                var counter = 0;
                while (!_client.IsConnected)
                {
                    if (counter >= 120)
                    {
                        Logger.Console.Log(Logger.Category.Twitch, $"No new connection in place. Waited for {counter} seconds. SendMessageAsync will return now."
                        + $"Therefore weren't able to send the message: {content} to the channel {_streamer.TwitchChannelName}.", true);
                        return;
                    }

                    Logger.Console.Log(Logger.Category.Twitch, $"No new connection in place. Waiting for 1 second until checking again. Waited for {counter} seconds now.");
                    Thread.Sleep(1000);
                    counter++;
                }
            }
            try
            {
                _client.SendMessage(_channelName, content);
            }
            catch (Exception e)
            {
                Logger.Console.Log(Logger.Category.Twitch, $"Something went wrong. Probably weren't able to send the message: {content} to the channel {_streamer.TwitchChannelName}."
                    + $"\nException: {e.Message}", true);
                return;
            }
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
            if ((message.StartsWith($"{commandPrefix}q ")
                || message.StartsWith($"{commandPrefix}question ")) && byCommand)
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
