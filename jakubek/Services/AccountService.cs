using jakubek.Entities;
using jakubek.Models;
using jakubek.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Services
{
    public class AccountService : IAccountService
    {
        private readonly BaseContext _context;
        private readonly IPasswordHasher<User> _hasher;
        public AccountService(BaseContext baseContext, IPasswordHasher<User> hasher)
        {
            _context = baseContext;
            _hasher = hasher;
        }
        public void RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            var user = new User()
            {
                Login = registerUserViewModel.Login,
                RoleId = registerUserViewModel.RoleId
            };
            var hashedPassword = _hasher.HashPassword(user, registerUserViewModel.Password);
            user.PasswordHash = hashedPassword;
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
