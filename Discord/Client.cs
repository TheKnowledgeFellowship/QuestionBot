using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using QuestionBot.CommandSystem;

namespace QuestionBot.Discord
{
    public class Client
    {
        private DSharpPlus.DiscordClient _discord;
        private CommandManager _commandManager;

        public Client(CommandManager commandManager)
        {
            var config = Config.Config.Load();
            _discord = new DSharpPlus.DiscordClient(new DiscordConfiguration()
            {
                Token = config.Discord.Token,
                TokenType = TokenType.Bot
            });

            _discord.MessageCreated += HandleMessageCreated;
            _commandManager = commandManager;
        }

        public async Task ConnectAsync()
        {
            await _discord.ConnectAsync();
            Logger.Console.Log(Logger.Category.Discord, "Connecting.");
        }

        public async Task DisconnectAsync()
        {
            await _discord.DisconnectAsync();
            Logger.Console.Log(Logger.Category.Discord, "Disconnecting.");
        }

        public async Task SendMessageAsync(ulong channelId, string content)
        {
            var channel = await _discord.GetChannelAsync(channelId);
            await _discord.SendMessageAsync(channel, content);
            Logger.Console.Log(Logger.Category.Discord, $"Sent message in {channelId}: {content}");
        }

        public async Task HandleMessageCreated(MessageCreateEventArgs args)
        {
            var commandText = args.Message.Content.Trim();
            if (commandText[0] == '!')
            {
                commandText = commandText.Remove(0, 1).Trim();
                await _commandManager.HandleCommandAsync(commandText, args, this);
            }
        }
    }
}
