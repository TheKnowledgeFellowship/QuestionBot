using System;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using TwitchLib.Client.Events;

namespace QuestionBot.Logger
{
    public enum Category
    {
        Discord,
        Twitch,
        BotIntern
    }

    public static class Console
    {
        public static void Log(Category category, string message, bool important = false)
        {
            var logMessage = "";

            if (important)
                logMessage += "!";

            logMessage += $"{DateTime.Now.ToString("s")} ";
            logMessage += $"[ {category} ] ";
            logMessage += message;

            System.Console.WriteLine(logMessage.ToString());
        }

        public static void LogCommand(string command, MessageCreateEventArgs args)
        {
            var logMessage = "";

            logMessage += $"Command: \"{command}\" ";
            logMessage += $"triggerd by: \"{args.Author.Username}\" ";
            logMessage += $"in: \"{args.Channel.Name}\" ";
            logMessage += $"in: \"{args.Guild.Name}\" ";
            logMessage += $"(message contents: \"{args.Message.Content}\") ";

            Log(Category.Discord, logMessage);
        }

        public static void LogCommand(string command, OnMessageReceivedArgs args)
        {
            var logMessage = "";

            logMessage += $"Command: \"{command}\" ";
            logMessage += $"triggerd by: \"{args.ChatMessage.Username}\" ";
            logMessage += $"on: \"{args.ChatMessage.Channel}\" ";
            logMessage += $"(message contents: \"{args.ChatMessage.Message}\") ";

            Log(Category.Twitch, logMessage);
        }
    }
}
