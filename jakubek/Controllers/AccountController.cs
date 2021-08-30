using jakubek.Models;
using jakubek.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet("user")]
        public ActionResult GetUserByToken()
        {
            if (Request.Cookies["jwt"] is null)
                return Unauthorized();
            string jwt = Request.Cookies["jwt"];
            UserViewModel userDto = _accountService.Verify(jwt);
            return Ok(userDto);
        }
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserViewModel registerUserViewModel)
        {
            _accountService.RegisterUser(registerUserViewModel);
            return Ok("Udało się utworzyć konto");
        }

        [HttpPost("login")]
        public ActionResult LoginUser([FromBody] LoginUserViewModel loginUserViewModel)
        {
            string token = _accountService.GenerateJwt(loginUserViewModel);
            Response.Cookies.Append("jwt", token, new Microsoft.AspNetCore.Http.CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure=true });
            return Ok(new { 
            message = $"Witaj {loginUserViewModel.Login}"
            });
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("jwt", new Microsoft.AspNetCore.Http.CookieOptions() { SameSite = SameSiteMode.None, Secure = true });

            return Ok(new { 
            message = "Zostałeś poprawnie wylogowany"
            });
        }
    }
}
