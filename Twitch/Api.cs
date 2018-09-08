using System.Threading.Tasks;
using TwitchLib.Api;

namespace QuestionBot.Twitch
{
    public class Api
    {
        private static TwitchAPI _api;

        public Api()
        {
            var config = Config.Config.Load();
            _api = new TwitchAPI();
            _api.Settings.ClientId = config.Twitch.ClientId;
            _api.Settings.AccessToken = config.Twitch.AccessToken;
        }

        public async Task<string> GetChannelIdFromChannelName(string channelName)
        {
            var result = await _api.Users.v5.GetUserByNameAsync(channelName);

            if (result.Total == 0)
                return null;

            return result.Matches[0].Id;
        }

        public async Task<bool> CheckStreamerOnlineStatus(string channelId) => await _api.Streams.v5.BroadcasterOnlineAsync(channelId);
    }
}
