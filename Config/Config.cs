using System.IO;
using Newtonsoft.Json;

namespace QuestionBot.Config
{
    public class Discord
    {
        public ulong Admin { get; set; }
        public string Token { get; set; }
    }

    public class Twitch
    {
        public string Username { get; set; }
        public string ClientId { get; set; }
        public string AccessToken { get; set; }
    }

    public class Config
    {
        public Discord Discord { get; set; }
        public Twitch Twitch { get; set; }

        private Config() { }

        public static Config Load() => JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
    }
}
