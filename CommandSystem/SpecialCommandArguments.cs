using DSharpPlus.EventArgs;

namespace QuestionBot.CommandSystem
{
    public class SpecialCommandArguments
    {
        public SpecialCommandArguments(string commandText, IPlatformClient client, Platform platform, MessageCreateEventArgs messageArgs)
        {
            this.CommandText = commandText;
            this.Client = client;
            this.Platform = platform;
            this.MessageArgs = messageArgs;
        }

        public string CommandText;
        public IPlatformClient Client;
        public Platform Platform;
        public MessageCreateEventArgs MessageArgs;
    }
}
