using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using QuestionBot.Discord.Commands;
using QuestionBot.ItemsJson;
using QuestionBot.Models;

namespace QuestionBot.Discord
{
    public class Client
    {
        private DSharpPlus.DiscordClient _discord;
        private CommandsNextModule _commands;
        private ItemsJson<StreamerId> _permittedStreamerIds;
        private ItemsJson<Streamer> _streamer;

        public Client(ItemsJson<Streamer> streamer)
        {
            _permittedStreamerIds = new ItemsJson<StreamerId>("PermittedStreamerIds.json");
            _streamer = streamer;

            var config = Config.Config.Load();
            _discord = new DSharpPlus.DiscordClient(new DiscordConfiguration()
            {
                Token = config.Discord.Token,
                TokenType = TokenType.Bot
            });

            DependencyCollection dep = null;
            using (var d = new DependencyCollectionBuilder())
            {
                d.AddInstance(new GeneralDependencies()
                {
                    PermittedStreamerIds = _permittedStreamerIds,
                    Streamer = _streamer
                });
                dep = d.Build();
            }

            _commands = _discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefix = "!",
                CaseSensitive = false,
                Dependencies = dep
            });

            _commands.RegisterCommands<Commands.General>();
        }

        public async Task ConnectAsync()
        {
            await _discord.ConnectAsync();
        }

        public async Task DisconnectAsync()
        {
            await _discord.DisconnectAsync();
        }

        public async Task SendMessageAsync(ulong channelId, string content)
        {
            var channel = await _discord.GetChannelAsync(channelId);
            await _discord.SendMessageAsync(channel, content);
        }
    }
}
