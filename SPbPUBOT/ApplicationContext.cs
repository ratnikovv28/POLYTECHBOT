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
            optionsBuilder.UseSqlServer("Data Source=mssql-78666-0.cloudclusters.net,19838;Initial Catalog=polytemporaryDB;User ID=Mainoper;Password=Mainoper0;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
