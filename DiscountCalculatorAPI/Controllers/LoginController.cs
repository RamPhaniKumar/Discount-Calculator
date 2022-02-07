using DiscountCalculatorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscountCalculatorAPI.Services;
using DiscountCalculatorAPI.Interface;
using DiscountCalculatorAPI.Data;
using System.ComponentModel.DataAnnotations;

namespace DiscountCalculatorAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class LoginController : ControllerBase
    {
        private readonly IUserDetails _user;

        public LoginController(IUserDetails user)
        {
            _user = user;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ObjectResult> Login([FromBody] LoginRequest req)
        {
            var result = await _user.Login(req);
            return Ok(result);
        }
    }
}
