using System.Threading.Tasks;

namespace QuestionBot.CommandSystem
{
    public interface ISpecialCommand
    {
        string Name { get; }
        string Call { get; }
        Platform Platform { get; }

        Task ActionAsync(SpecialCommandArguments commandArguments);
    }
}
