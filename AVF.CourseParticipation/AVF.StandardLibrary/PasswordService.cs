using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AVF.StandardLibrary
{
    public class PasswordService : IPasswordService
    {
        public void Validate()
        {
            var passwordHasher = new PasswordHasher<object>();

            var password = "Pass123$";

            var hash = passwordHasher.HashPassword(null, password);

            //For example from Identity database
            var hashedPassword = "AQAAAAEAACcQAAAAEMi2a2jQSPEvrETmqpp6nKdusVeqlE6pODGH0wtwz4D/BwR6fzD/yaTK87QV23xXDA==";

            //Password entered by user now
            var providedPassword = "Pass123$";

            var passwordVerficationResult = passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        }

        public Task<bool> IsValidAsync(string enteredPassword, string storedPasswordHash, string pepper)
        {
            var passwordHasher = new PasswordHasher<object>();
            var passwordVerficationResult = passwordHasher.VerifyHashedPassword(null, storedPasswordHash, enteredPassword);
            if (passwordVerficationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                throw new Exception("PasswordVerificationResult.SuccessRehashNeeded");
            }
            return Task.FromResult(passwordVerficationResult == PasswordVerificationResult.Success);
        }

        public Task<string> HashPasswordAsync(string password, string pepper, byte[] saltBytes = null)
        {
            var passwordHasher = new PasswordHasher<object>();
            var hash = passwordHasher.HashPassword(null, password);
            return Task.FromResult(hash);
        }
    }
}
