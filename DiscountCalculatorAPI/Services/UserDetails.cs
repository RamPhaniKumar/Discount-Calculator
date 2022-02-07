using DiscountCalculatorAPI.Data;
using DiscountCalculatorAPI.Interface;
using DiscountCalculatorAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DiscountCalculatorAPI.Services
{
    public class UserDetails : IUserDetails
    {
        private readonly ApplicationDBContext dbContext;
        const int SaltLength = 16;
        const int HashLength = 20;
        public UserDetails(ApplicationDBContext context)
        {
            dbContext = context;
        }

        public async Task<string> Login(LoginRequest req)
        {
            var userinfo = dbContext.UserDetails.FirstOrDefault(x => x.UserName == req.UserName);

            if (userinfo == null)
            {
                throw new ArgumentException("User Not Found");
            }

            if (!Validate(userinfo.PasswordHash, req.Password))
            {
                throw new ArgumentException("Invalid password");
            }

            userinfo.AccessKey = Guid.NewGuid().ToString();

            await dbContext.SaveChangesAsync();

            return userinfo.AccessKey;
        }
        public bool ValidateAccessKey(string key)
        {
            var userinfo = dbContext.UserDetails.FirstOrDefault(x => x.AccessKey == key);
            return userinfo != null;
        }
        private static bool Validate(string savedHash, string password)
        {
            try
            {
                var hashBytes = Convert.FromBase64String(savedHash);
                var salt = new byte[SaltLength];
                Array.Copy(hashBytes, 0, salt, 0, SaltLength);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 5000))
                {
                    var hash = pbkdf2.GetBytes(HashLength);

                    for (int i = 0; i < HashLength; i++)
                        if (hashBytes[i + SaltLength] != hash[i])
                            return false;
                }
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

    }
}
