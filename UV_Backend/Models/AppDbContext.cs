using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UV_Backend.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionAnswer> questionAnswers { get; set; }
        public DbSet<UV> UVData { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
