using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using QuestionBot.Discord.Commands;
using QuestionBot.ItemsJson;
using QuestionBot.Models;
using System.Collections.Generic;

namespace QuestionBot.Discord
{
    public class Client
    {
        private DSharpPlus.DiscordClient _discord;
        private CommandsNextModule _commands;
        private ItemsJson<StreamerId> _permittedStreamerIds;
        private ItemsJson<Streamer> _streamer;

        public CommandsEvents CommandsEvents { get; private set; }

        public Client(ItemsJson<Streamer> streamer, Dictionary<ulong, ItemsJson<Models.Question>> questions)
        {
            _permittedStreamerIds = new ItemsJson<StreamerId>("PermittedStreamerIds.json");
            _streamer = streamer;

            var config = Config.Config.Load();
            _discord = new DSharpPlus.DiscordClient(new DiscordConfiguration()
            {
                Token = config.Discord.Token,
                TokenType = TokenType.Bot
            });

            CommandsEvents = new CommandsEvents();
            DependencyCollection dep = null;
            using (var d = new DependencyCollectionBuilder())
            {
                d.AddInstance(new GeneralDependencies()
                {
                    PermittedStreamerIds = _permittedStreamerIds,
                    Streamer = _streamer,
                    CommandsEvents = CommandsEvents
                });
                d.AddInstance(new QuestionDependencies()
                {
                    Questions = questions
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
            _commands.RegisterCommands<Commands.Question>();
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
