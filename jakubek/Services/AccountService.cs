using jakubek.Entities;
using jakubek.Exceptions;
using jakubek.Models;
using jakubek.Services.Interfaces;
using jakubek.Repositories;
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
using jakubek.Repositories.Interfaces;

namespace jakubek.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordHasher<User> _hasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IAccountRepository _accountRepository;
        public AccountService(IPasswordHasher<User> hasher, AuthenticationSettings authenticationSettings, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
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
            _accountRepository.Create(user);
            _accountRepository.SaveChanges();
        }

        public string GenerateJwt(LoginUserViewModel loginViewModel)
        {
            var user = _accountRepository.GetUserByLogin(loginViewModel.Login, u => u.Role);

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

        public UserViewModel Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters() {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var user = _accountRepository.GetById(userId);

            var userDto = new UserViewModel()
            {
                Login = user.Login,
                Role = user.RoleId == 1 ? "Użytkownik" : "Administrator"
            };

            return userDto;
        }
    }
}
