using DiscountCalculatorAPI.Models;
using DiscountCalculatorAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DiscountCalculatorAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        const int SaltLength = 16;
        const int HashLength = 20;
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> context) 
            : base(context)
        {
            LoadUserInfo();
        }
        public DbSet<UserInfo> UserDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        private void LoadUserInfo()
        {
            if (UserDetails.FirstOrDefault(x => x.UserName == "testuser") != null)
                return;
            var user = new UserInfo()
            {
                UserName = "testuser",
                PasswordHash = GetHash("test")
            };
            UserDetails.Add(user);
            this.SaveChanges();
        }


        private static string GetHash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltLength]);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 5000))
            {
                var hash = pbkdf2.GetBytes(HashLength);
                var hashBytes = new byte[SaltLength + HashLength];
                Array.Copy(salt, 0, hashBytes, 0, SaltLength);
                Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);
                return Convert.ToBase64String(hashBytes);
            }
        }

    }
}
