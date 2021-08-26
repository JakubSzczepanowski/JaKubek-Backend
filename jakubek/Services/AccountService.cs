using jakubek.Entities;
using jakubek.Exceptions;
using jakubek.Models;
using jakubek.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Services
{
    public class AccountService : IAccountService
    {
        private readonly BaseContext _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly AuthenticationSettings _authenticationSettings;
        public AccountService(BaseContext baseContext, IPasswordHasher<User> hasher, AuthenticationSettings authenticationSettings)
        {
            _context = baseContext;
            _hasher = hasher;
            _authenticationSettings = authenticationSettings;
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

        public string GenerateJwt(LoginUserViewModel loginViewModel)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == loginViewModel.Login);

            if (user is null)
                throw new BadRequestException("Niepoprawny login lub hasło");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, loginViewModel.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Niepoprawny login lub hasło");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
