using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Abstractions
{
    public interface IPersistance
    {
        IUserRepository UserRepository { get; set; }
        IPasswordRepository AuthorizationRepository { get; set; }
        IPasswordRepository PasswordRepository { get; set; }

        IDisposable BeginTransaction();
        void TransactionCommit();
        void TransactionRollback();

        int SaveChanges();
    }
}
