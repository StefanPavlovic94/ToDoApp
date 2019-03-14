namespace UserManagement.WebApi.Models
{
    public class CreateUserViewModel : UserViewModel
    {
        public string Password { get; set; }
        public string PasswordRetype { get; set; }
    }
}