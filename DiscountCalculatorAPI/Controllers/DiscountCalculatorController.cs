using DiscountCalculatorAPI.Interface;
using DiscountCalculatorAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCalculatorController : ControllerBase
    {
        private readonly IDiscountCalculator _discount;
        private readonly IUserDetails _user;
        public DiscountCalculatorController(IDiscountCalculator discountCalculator, IUserDetails user)
        {
            _discount = discountCalculator;
            _user = user;
        }

        [HttpPost]
        [Route("CalculateTotalPrice")]
        public async Task<ObjectResult> Discount([FromHeader(Name = "x-AccessKey")][Required] string AccessKey,
            [FromBody] PriceRequest req)
        {
            if (_user.ValidateAccessKey(AccessKey))
            {
                return Ok(await _discount.CalculateTotalPrice(req));
            }
            return BadRequest("Inavlid accesskey");
        }
    }
}
