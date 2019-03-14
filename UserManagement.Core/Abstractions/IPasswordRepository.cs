using UserManagement.Core.Model;

namespace UserManagement.Core.Abstractions
{
    public interface IPasswordRepository
    {
        void CreatePassword(Password password);
        Password GetPasswordInfo(string email);
    }
}
