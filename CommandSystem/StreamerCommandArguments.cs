using System.Collections.Generic;
using QuestionBot.Models;

namespace QuestionBot.CommandSystem
{
    public class StreamerCommandArguments
    {
        public StreamerCommandArguments(string commandText, IPlatformClient client, Streamer streamer, PermissionLevel reachedPermissionLevel, Platform platform)
        {
            this.CommandText = commandText;
            this.Client = client;
            this.Streamer = streamer;
            this.ReachedPermissionLevel = reachedPermissionLevel;
            this.Platform = platform;
        }

        public string CommandText;
        public IPlatformClient Client;
        public Streamer Streamer;
        public PermissionLevel ReachedPermissionLevel;
        public Platform Platform;
    }
}
