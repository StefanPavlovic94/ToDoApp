using Effort;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Model;

namespace UserManagement.Persistance.Implementations
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }

        public UserContext() 
            : base(ConfigurationManager.ConnectionStrings["UserDatabase"].ConnectionString)
        {
        }

        public UserContext(DbConnection connection) : base(connection, false)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static UserContext AsInMemoryDatabase()
        {
            var connection = DbConnectionFactory.CreateTransient();

            return new UserContext(connection);
        }
    }
}
