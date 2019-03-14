using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;

namespace UserManagement.Persistance.Implementations
{
    public class Persistance : IPersistance
    {
        private UserContext userContext;
        private DbContextTransaction dbTransaction;

        public IUserRepository UserRepository { get; set; }
        public IAuthorizationRepository AuthorizationRepository { get; set; }

        public Persistance(UserContext userContext)
        {
            this.userContext = userContext;

            this.UserRepository = new UserRepository(userContext);
            this.AuthorizationRepository = new AuthorizationRepository(userContext);
        }
        
        public IDisposable BeginTransaction()
        {
            this.dbTransaction = this.userContext.Database.BeginTransaction();
            return this.dbTransaction;
        }

        public int SaveChanges()
        {
            return this.userContext.SaveChanges();
        }

        public void TransactionCommit()
        {
            if (this.dbTransaction == null)
            {
                throw new Exception("Transaction scope null exception");
            }

            this.userContext.SaveChanges();

            this.dbTransaction.Commit();
            this.dbTransaction.Dispose();
        }

        public void TransactionRollback()
        {
            if (this.dbTransaction == null)
            {
                throw new Exception("Transaction scope null exception");
            }

            this.dbTransaction.Rollback();
            this.dbTransaction.Dispose();
        }
    }
}
