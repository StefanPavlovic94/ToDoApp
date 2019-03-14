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
        IAuthorizationRepository AuthorizationRepository { get; set; }

        int SaveChanges();

        IDisposable BeginTransaction();
        void TransactionCommit();
    }
}
