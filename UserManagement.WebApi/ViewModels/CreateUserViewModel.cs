namespace UserManagement.WebApi.ViewModels
{
    public class CreateUserViewModel : UserViewModel
    {
        public string Password { get; set; }
        public string PasswordRetype { get; set; }
    }
}