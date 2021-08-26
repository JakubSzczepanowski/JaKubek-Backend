using jakubek.Models;
using jakubek.Services.Interfaces;
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
            return Ok(token);
        }
    }
}
