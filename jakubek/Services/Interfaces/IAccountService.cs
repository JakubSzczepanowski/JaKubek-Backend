using jakubek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Services.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserViewModel registerUserViewModel);
    }
}
