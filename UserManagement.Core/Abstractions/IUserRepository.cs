using UserManagement.Core.Model;

namespace UserManagement.Core.Abstractions
{
    public interface IUserRepository
    {
        User GetUser(int userId);
        User CreateUser(User user);
        User EditUser(User user);
        User DeleteUser(int userId);
    }
}
