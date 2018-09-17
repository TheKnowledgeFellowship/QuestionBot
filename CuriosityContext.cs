using Microsoft.EntityFrameworkCore;
using QuestionBot.Models;

namespace QuestionBot
{
    public class CuriosityContext : DbContext
    {
        public DbSet<PermittedStreamer> PermittedStreamers { get; set; }
        public DbSet<Streamer> Streamer { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Models.Command> Commands { get; set; }
        public DbSet<Moderator> Moderators { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Curiosity.db");
        }
    }
}
