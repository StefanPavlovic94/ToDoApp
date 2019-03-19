using UserManagement.Core.Abstractions;

namespace UserManagement.Implementation.Services
{
    public class AuthenticationValidationService : IAuthenticationValidationService
    {
        public bool ValidateLogin(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return true;
        }
    }
}
