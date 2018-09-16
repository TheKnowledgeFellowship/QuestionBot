using System.Threading.Tasks;

namespace QuestionBot.CommandSystem
{
    public interface IPlatformClient
    {
        Task SendMessageAsync(string content);
    }
}
