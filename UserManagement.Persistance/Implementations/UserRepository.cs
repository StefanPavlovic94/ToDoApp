using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Model;

namespace UserManagement.Persistance.Implementations
{
    public class UserRepository : IUserRepository
    {
        private UserContext userContext;

        public UserRepository(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public User CreateUser(User user)
        {
            return this.userContext.Users.Add(user);
        }

        public User DeleteUser(int userId)
        {
            var user = this.userContext.Users.Single(u => u.Id == userId);
            this.userContext.Users.Remove(user);
            return user;
        }

        public User EditUser(User user)
        {
            var userToBeEdited = this.userContext.Users.Single(u => u.Id == user.Id);

            userToBeEdited.FirstName = user.FirstName;
            userToBeEdited.LastName = user.LastName;
            userToBeEdited.Email = user.Email;

            this.userContext.Entry(userToBeEdited).State = System.Data.Entity.EntityState.Modified;
            return userToBeEdited;
        }

        public User GetUser(int userId)
        {
            return this.userContext.Users.Single(u => u.Id == userId);
        }
    }
}
