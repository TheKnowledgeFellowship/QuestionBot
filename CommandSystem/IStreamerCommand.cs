using System.Threading.Tasks;

namespace QuestionBot.CommandSystem
{
    public interface IStreamerCommand
    {
        string Name { get; }
        string Call { get; }
        PermissionLevel TwitchPermissionLevel { get; }
        PermissionLevel DiscordPermissionLevel { get; }
        Platform Platform { get; }

        Task ActionAsync(StreamerCommandArguments commandArguments);
    }
}
