using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

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

        public static void LogCommand(string command, CommandContext context)
        {
            var logMessage = "";

            logMessage += $"Command: \"{command}\" ";
            logMessage += $"triggerd by: \"{context.Message.Author}\" ";
            logMessage += $"in: \"{context.Message.Channel}\" ";
            logMessage += $"in: \"{context.Guild}\" ";
            logMessage += $"(message contents: \"{context.Message.Content}\") ";

            Log(Category.Discord, logMessage.ToString());
        }

        public static async Task ResponseLogAsync(string content, CommandContext context)
        {
            await context.RespondAsync(content);
            Console.Log(Logger.Category.Discord, $"Response: \"{content}\"");
        }
    }
}
