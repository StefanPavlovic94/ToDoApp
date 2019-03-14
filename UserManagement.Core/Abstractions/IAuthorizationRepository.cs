using UserManagement.Core.Model;

namespace UserManagement.Core.Abstractions
{
    public interface IAuthorizationRepository
    {
        void CreatePassword(Password password);
        Password GetPasswordInfo(string email);
    }
}
