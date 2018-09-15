using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using QuestionBot.Discord.Commands;
using QuestionBot.Models;
using System.Collections.Generic;

namespace QuestionBot.Discord
{
    public class Client
    {
        private DSharpPlus.DiscordClient _discord;
        private CommandsNextModule _commands;

        public CommandsEvents CommandsEvents { get; private set; }

        public Client()
        {
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
                    CommandsEvents = CommandsEvents
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
            _commands.RegisterCommands<Commands.Configuration>();
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
    }
}
