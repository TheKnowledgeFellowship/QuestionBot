using System;
using System.Threading.Tasks;

namespace QuestionBot
{
    class Program
    {
        static void Main(string[] args) => MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            var bot = new Bot();
            await bot.StartAsync();
            await Task.Delay(-1);
        }
    }
}
