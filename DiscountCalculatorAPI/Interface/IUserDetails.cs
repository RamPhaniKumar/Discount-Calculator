using DiscountCalculatorAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountCalculatorAPI.Interface
{
    public interface IUserDetails
    {
        Task<string> Login(LoginRequest req);
        bool ValidateAccessKey(string key);
    }
}
