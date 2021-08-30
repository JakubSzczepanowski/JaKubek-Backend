using jakubek.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Services.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserViewModel registerUserViewModel);
        string GenerateJwt(LoginUserViewModel loginViewModel);
        UserViewModel Verify(string jwt);
    }
}
