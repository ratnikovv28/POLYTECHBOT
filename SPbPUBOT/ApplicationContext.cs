using Microsoft.EntityFrameworkCore;

namespace SPbPUBOT
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserAssistance> UserAssistance { get; set; }

        public DbSet<Operator> Operators { get; set; }

        public ApplicationContext()
        {
            Database.EnsureDeleted();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=polybotdb;Trusted_Connection=True;");
        }
    }
}
