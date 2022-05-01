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
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=mssql-77678-0.cloudclusters.net,12502;Initial Catalog=polytemporaryDB;Persist Security Info=True;User ID=MainOpeartor;Password=Mainoper0");
        }
    }
}
