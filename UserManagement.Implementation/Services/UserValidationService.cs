using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Common.Extensions;
using UserManagement.Core.Abstractions;
using UserManagement.Core.DomainModel;

namespace UserManagement.Implementation.Services
{
    public class UserValidationService : IUserValidationService
    {
        public bool ValidateEditUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName))
            {
                return false;
            }

            return true;
        }

        public bool ValidateRegisterUserParams(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(password) ||
                !user.Email.IsValidEmail())
            {
                return false;
            }

            return true;
        }
    }
}
