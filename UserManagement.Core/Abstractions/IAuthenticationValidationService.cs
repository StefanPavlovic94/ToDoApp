namespace UserManagement.Core.Abstractions
{
    public interface IAuthenticationValidationService
    {
        bool ValidateLogin(string email, string password);
    }
}
