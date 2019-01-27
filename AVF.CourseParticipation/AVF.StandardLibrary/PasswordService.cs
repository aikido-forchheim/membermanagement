using System;
using Microsoft.AspNetCore.Identity;

namespace AVF.StandardLibrary
{
    public class PasswordService
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
    }
}
